using Microsoft.VisualStudio;

namespace VSMonoSDB.Debugging.Enumerators
{
    public class BaseEnumerator<TObjectType, TEnumType> where TEnumType : class
    {
        private readonly TObjectType[] _data;
        private uint _position;

        public BaseEnumerator(TObjectType[] data)
        {
            _data = data;
            _position = 0;
        }

        public int Clone(out TEnumType ppEnum)
        {
            ppEnum = null;
            return VSConstants.E_NOTIMPL;
        }

        public int GetCount(out uint pcelt)
        {
            pcelt = (uint)_data.Length;
            return VSConstants.S_OK;
        }

        public int NextOut(uint celt, TObjectType[] rgelt, out uint celtFetched)
        {
            return Move(celt, rgelt, out celtFetched);
        }

        public int Next(uint celt, TObjectType[] rgelt, ref uint celtFetched)
        {
            return Move(celt, rgelt, out celtFetched);
        }

        public int Reset()
        {
            lock (this)
            {
                _position = 0;

                return VSConstants.S_OK;
            }
        }

        public int Skip(uint celt)
        {
            uint celtFetched;

            return Move(celt, null, out celtFetched);
        }

        private int Move(uint celt, TObjectType[] rgelt, out uint celtFetched)
        {
            lock (this)
            {
                var hr = VSConstants.S_OK;
                celtFetched = (uint)_data.Length - _position;

                if (celt > celtFetched)
                    hr = VSConstants.S_FALSE;
                else if (celt < celtFetched)
                    celtFetched = celt;

                if (rgelt != null)
                    for (var c = 0; c < celtFetched; c++)
                        rgelt[c] = _data[_position + c];

                _position += celtFetched;

                return hr;
            }
        }
    }
}
