namespace ThanalSoft.SmartComplex.Business
{
    public abstract class BaseBusiness<TBussinessType> where TBussinessType : new()
    {
        private static TBussinessType _bussinessType;
        public static TBussinessType Instance 
        {
            get
            {
                if(_bussinessType == null)
                    _bussinessType = new TBussinessType();
                return _bussinessType;
            }
        }
    }
}