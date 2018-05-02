using System;
using Microsoft.VisualStudio.Debugger.Interop;
using static Microsoft.VisualStudio.VSConstants;
using Mono.Debugging.Client;
using System.IO;
using VSMonoSDB.Debugging.Enumerators;
using System.Linq;

namespace VSMonoSDB.Debugging
{
    public class MonoStackFrame : IDebugStackFrame2, IDebugExpressionContext2
    {
        private static Guid FilterLocalsPlusArgs = new Guid("e74721bb-10c0-40f5-807f-920d37f95419");
        private static Guid FilterAllLocalsPlusArgs = new Guid("939729a8-4cb0-4647-9831-7ff465240d5f");
        private static Guid FilterAllLocals = new Guid("196db21f-5f22-45a9-b5a3-32cddb30db06");
        private static Guid FilterLocals = new Guid("b200f725-e725-4c53-b36a-1ec27aef12ef");
        private static Guid FilterArgs = new Guid("804bccea-0475-4ae7-8a46-1862688ab863");

        private MonoEngine _engine;
        private MonoThread _thread;

        private StackFrame _monoStackFrame;

        public MonoStackFrame(MonoEngine engine, MonoThread thread, StackFrame frame)
        {
            _engine = engine;
            _thread = thread;
            _monoStackFrame = frame;
        }

        public int EnumProperties(uint dwFields, uint nRadix, ref Guid guidFilter, uint dwTimeout, out uint pcelt, out IEnumDebugPropertyInfo2 ppEnum)
        {
            if (guidFilter == FilterLocalsPlusArgs || guidFilter == FilterAllLocalsPlusArgs || guidFilter == FilterAllLocals)
            {
                EnumLocalsAndArgs(out pcelt, out ppEnum);
                return S_OK;
            }
            else if (guidFilter == FilterLocals)
            {
                EnumLocals(out pcelt, out ppEnum);
                return S_OK;
            }
            else if (guidFilter == FilterArgs)
            {
                EnumParameters(out pcelt, out ppEnum);
                return S_OK;
            }

            ppEnum = null;
            pcelt = 0;

            return E_NOTIMPL;
        }

        private void EnumLocalsAndArgs(out uint elementsReturned, out IEnumDebugPropertyInfo2 enumObject)
        {
            elementsReturned = 0;

            ObjectValue[] locals = _monoStackFrame.GetLocalVariables(EvaluationOptions.DefaultOptions);

            int offset = 0;

            if (locals != null)
            {
                elementsReturned = (uint)locals.Length;
                offset = locals.Length;
            }

            ObjectValue[] parameters = _monoStackFrame.GetParameters(EvaluationOptions.DefaultOptions);
            if (parameters != null)
            {
                elementsReturned += (uint)parameters.Length;
            }

            var propInfo = new DEBUG_PROPERTY_INFO[elementsReturned];

            if (locals != null)
            {
                for (int i = 0; i < locals.Length; i++)
                {
                    MonoProperty property = new MonoProperty(null, locals[i], locals[i].Name);
                    propInfo[i] = property.CreatePropertyInfo((uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_STANDARD);
                }
            }

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    MonoProperty property = new MonoProperty(null, parameters[i], parameters[i].Name);
                    propInfo[offset + i] = property.CreatePropertyInfo((uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_STANDARD);
                }
            }

            enumObject = new MonoPropertyEnumerator(propInfo);
        }

        private void EnumLocals(out uint elementsReturned, out IEnumDebugPropertyInfo2 enumObject)
        {
            ObjectValue[] locals = _monoStackFrame.GetLocalVariables(EvaluationOptions.DefaultOptions);

            elementsReturned = (uint)locals.Length;
            DEBUG_PROPERTY_INFO[] propInfo = new DEBUG_PROPERTY_INFO[locals.Length];

            for (int i = 0; i < propInfo.Length; i++)
            {
                MonoProperty property = new MonoProperty(null, locals[i], locals[i].Name);
                propInfo[i] = property.CreatePropertyInfo((uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_STANDARD);
            }

            enumObject = new MonoPropertyEnumerator(propInfo);
        }

        private void EnumParameters(out uint elementsReturned, out IEnumDebugPropertyInfo2 enumObject)
        {
            ObjectValue[] parameters = _monoStackFrame.GetParameters(EvaluationOptions.DefaultOptions);

            elementsReturned = (uint)parameters.Length;
            DEBUG_PROPERTY_INFO[] propInfo = new DEBUG_PROPERTY_INFO[parameters.Length];

            for (int i = 0; i < propInfo.Length; i++)
            {
                MonoProperty property = new MonoProperty(null, parameters[i], parameters[i].Name);
                propInfo[i] = property.CreatePropertyInfo((uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_STANDARD);
            }

            enumObject = new MonoPropertyEnumerator(propInfo);
        }

        public int GetCodeContext(out IDebugCodeContext2 ppCodeCxt)
        {
            ppCodeCxt = new MonoMemoryContext(null, (uint)_monoStackFrame.Address);

            return S_OK;
        }

        public int GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            ppProperty = null;

            return E_NOTIMPL;
        }

        public int GetDocumentContext(out IDebugDocumentContext2 ppCxt)
        {
            if (_monoStackFrame.HasDebugInfo)
            {
                ppCxt = new MonoDocumentContext(new TextPositionInfo(
                    _monoStackFrame.SourceLocation.FileName,
                    _monoStackFrame.SourceLocation.Line - 1,
                    _monoStackFrame.SourceLocation.Column - 1), null);

                return S_OK;
            }

            ppCxt = null;
            return S_FALSE;
        }

        public int GetExpressionContext(out IDebugExpressionContext2 ppExprCxt)
        {
            ppExprCxt = this;

            return S_OK;
        }

        public FRAMEINFO GetFrameInfo(uint dwFieldSpec)
        {
            FRAMEINFO frameInfo = new FRAMEINFO();

            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_FUNCNAME) != 0)
            {
                if (_monoStackFrame.HasDebugInfo)
                {
                    ObjectValue[] parameters = _monoStackFrame.GetParameters(EvaluationOptions.DefaultOptions);

                    frameInfo.m_bstrFuncName = "";

                    if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_MODULE) != 0)
                        frameInfo.m_bstrFuncName = Path.GetFileName(_monoStackFrame.FullModuleName) + "!";

                    frameInfo.m_bstrFuncName += _monoStackFrame.SourceLocation.MethodName;

                    if (((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS) != 0) && (parameters.Length > 0))
                    {
                        frameInfo.m_bstrFuncName += "(";
                        for (var i = 0; i < parameters.Length; i++)
                        {
                            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_TYPES) != 0)
                                frameInfo.m_bstrFuncName += parameters[i].TypeName + " ";

                            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_NAMES) != 0)
                                frameInfo.m_bstrFuncName += parameters[i].Name;

                            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_VALUES) != 0)
                                frameInfo.m_bstrFuncName += "=" + parameters[i].Value;

                            if (i < parameters.Length - 1)
                                frameInfo.m_bstrFuncName += ", ";
                        }
                        frameInfo.m_bstrFuncName += ")";
                    }

                    if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_LINES) != 0)
                        frameInfo.m_bstrFuncName += " Line:" + (uint)_monoStackFrame.SourceLocation.Line;
                }
                else
                {
                    frameInfo.m_bstrFuncName = _monoStackFrame.AddressSpace;
                }
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_FUNCNAME;
            }

            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_MODULE) != 0)
            {
                frameInfo.m_bstrModule = _monoStackFrame.FullModuleName;
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_MODULE;
            }

            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_STACKRANGE) != 0)
            {
                frameInfo.m_addrMin = (ulong)_monoStackFrame.Address;
                frameInfo.m_addrMax = (ulong)_monoStackFrame.Address;
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_STACKRANGE;
            }

            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_FRAME) != 0)
            {
                frameInfo.m_pFrame = this;
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_FRAME;
            }

            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_DEBUGINFO) != 0)
            {
                frameInfo.m_fHasDebugInfo = _monoStackFrame.HasDebugInfo ? 1 : 0;
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_DEBUGINFO;
            }

            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_STALECODE) != 0)
            {
                frameInfo.m_fStaleCode = 0;
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_STALECODE;
            }

            return frameInfo;
        }

        public int GetInfo(uint dwFieldSpec, uint nRadix, FRAMEINFO[] pFrameInfo)
        {
            pFrameInfo[0] = GetFrameInfo(dwFieldSpec);

            return S_OK;
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pguidLanguage = new Guid("{694DD9B6-B865-4C5B-AD85-86356E9C88DC}");
            pbstrLanguage = "C#";

            return S_OK;
        }

        public int GetName(out string pbstrName)
        {
            pbstrName = _monoStackFrame.SourceLocation.MethodName;

            return S_OK;
        }

        public int GetPhysicalStackRange(out ulong paddrMin, out ulong paddrMax)
        {
            paddrMin = 0;
            paddrMax = 0;

            return S_OK;
        }

        public int GetThread(out IDebugThread2 ppThread)
        {
            ppThread = _thread;

            return S_OK;
        }

        public int ParseText(string pszCode, uint dwFlags, uint nRadix, out IDebugExpression2 ppExpr, out string pbstrError, out uint pichError)
        {
            pbstrError = null;
            pichError = 0;

			StackFrame frame = _monoStackFrame;

			if (frame.ValidateExpression(pszCode))
            {
				ObjectValue evalValue = null;

				//TODO: Something is wrong with expression resolving, sometimes the result is the expression, not the result (???)
				if (!pszCode.Contains(" "))
				{
					ObjectValue possibleValue = FindPossibleValue(frame, pszCode);
					if (possibleValue != null)
					{
						evalValue = possibleValue;
					}
					else
					{
						evalValue = frame.GetExpressionValue(pszCode, EvaluationOptions.DefaultOptions);
					}
				}
				else
				{
					evalValue = frame.GetExpressionValue(pszCode, EvaluationOptions.DefaultOptions);
				}

				ppExpr = new MonoExpression(_engine, _thread, evalValue, pszCode);

                return S_OK;
            }

            ppExpr = null;
            return S_FALSE;
        }

		private ObjectValue FindPossibleValue(StackFrame frame, string name)
		{
			ObjectValue val = null;

			ObjectValue[] locals = frame.GetAllLocals(EvaluationOptions.DefaultOptions);
			val = locals.SingleOrDefault(x => x.Name == name);
			if (val != null)
				return val;

			return null;
		}
    }
}
