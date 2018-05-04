using Microsoft.VisualStudio.Debugger.Interop;
using Mono.Debugging.Client;
using System;
using VSMonoSDB.Debugging.Enumerators;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    public class MonoProperty : IDebugProperty2
    {
        private MonoProperty _parentProperty;
        private ObjectValue _value;
        private string _expression;

        public MonoProperty(MonoProperty parent, ObjectValue value, string expression)
        {
            _parentProperty = parent;
            _value = value;
            _expression = expression;
        }

        public int EnumChildren(uint dwFields, uint dwRadix, ref Guid guidFilter, ulong dwAttribFilter, string pszNameFilter, uint dwTimeout, out IEnumDebugPropertyInfo2 ppEnum)
        {
            ppEnum = null;

			ObjectValue[] children = _value.GetAllChildren();

			if (!_value.HasChildren && children.Length > 0)
                return S_FALSE;

            DEBUG_PROPERTY_INFO[] props = new DEBUG_PROPERTY_INFO[children.Length];

            for (var i = 0; i < children.Length; i++)
            {
                MonoProperty monoProp = new MonoProperty(this, children[i], _expression);

                props[i] = monoProp.CreatePropertyInfo(dwFields);
            }

            ppEnum = new MonoPropertyEnumerator(props);

            return S_OK;
        }

        public int GetDerivedMostProperty(out IDebugProperty2 ppDerivedMost)
        {
            ppDerivedMost = null;

            return E_NOTIMPL;
        }

        public int GetExtendedInfo(ref Guid guidExtendedInfo, out object pExtendedInfo)
        {
            pExtendedInfo = null;

            return E_NOTIMPL;
        }

        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            ppMemoryBytes = null;

            return E_NOTIMPL;
        }

        public int GetMemoryContext(out IDebugMemoryContext2 ppMemory)
        {
            ppMemory = null;

            return E_NOTIMPL;
        }

        public int GetParent(out IDebugProperty2 ppParent)
        {
            ppParent = _parentProperty;

            return E_NOTIMPL;
        }

        public DEBUG_PROPERTY_INFO CreatePropertyInfo(uint dwFields)
        {
            DEBUG_PROPERTY_INFO info = new DEBUG_PROPERTY_INFO();

            if ((dwFields & (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME) != 0)
            {
                info.bstrFullName = _expression;
                info.dwFields |= (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME;
            }

            if ((dwFields & (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME) != 0)
            {
                info.bstrName = _value.Name;
                info.dwFields |= (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME;
            }

            if ((dwFields & (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE) != 0)
            {
                info.bstrType = _value.TypeName;
                info.dwFields |= (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE;
            }

            if ((dwFields & (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE) != 0)
            {
				if(_value.HasFlag(ObjectValueFlags.Namespace) && _value.HasFlag(ObjectValueFlags.Object))
				{
					info.bstrValue = "Unavailable";
				}
				else if (_value.IsError)
				{
					info.bstrValue = "Evaluation error; " + _value.DisplayValue;
				}
				else if (_value.IsUnknown)
				{
					info.bstrValue = "Unknown value";
				}
				else
				{
					info.bstrValue = _value.DisplayValue;
				}
                info.dwFields |= (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE;
            }

            if ((dwFields & (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB) != 0)
            {
                info.dwAttrib |= (uint)0x0000000000000010; //enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_READONLY    

                if (_value.HasChildren)
                    info.dwAttrib |= (uint)0x0000000000000001; //enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_OBJ_IS_EXPANDABLE
            }

            if (((dwFields & (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP) != 0) || _value.HasChildren)
            {
                info.pProperty = this;
                info.dwFields |= (uint)enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP;
            }

            return info;
        }

        public int GetPropertyInfo(uint dwFields, uint dwRadix, uint dwTimeout, IDebugReference2[] rgpArgs, uint dwArgCount, DEBUG_PROPERTY_INFO[] pPropertyInfo)
        {
            pPropertyInfo[0] = CreatePropertyInfo(dwFields);

            return S_OK;
        }

        public int GetReference(out IDebugReference2 ppReference)
        {
            ppReference = null;

            return E_NOTIMPL;
        }

        public int GetSize(out uint pdwSize)
        {
            pdwSize = 0;

            return E_NOTIMPL;
        }

        public int SetValueAsReference(IDebugReference2[] rgpArgs, uint dwArgCount, IDebugReference2 pValue, uint dwTimeout)
        {
            return E_NOTIMPL;
        }

        public int SetValueAsString(string pszValue, uint dwRadix, uint dwTimeout)
        {
            _value.SetValue(pszValue);

            return S_OK;
        }
    }
}
