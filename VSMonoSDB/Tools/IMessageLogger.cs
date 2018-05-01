namespace VSMonoSDB.Tools
{
    public interface IMessageLogger
    {
        void InfoMessage(string info, string title);
        void ErrorMessage(string error, string title);
    }
}
