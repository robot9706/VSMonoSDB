using System;

namespace VSMonoSDB.Debugging
{
    public static class DebuggerGuids
    {
        public static string EngineName = "MonoSDB";

        public const string EngineIdGuidString = "49E6DA0F-30E1-42DD-9F95-B5F3C1C05465";
        public static readonly Guid EngineIdGuid = new Guid(EngineIdGuidString);

        public const string ProgramProviderString = "E2BCF3C0-F277-465E-BA98-B2AE540A4364";
        public static readonly Guid ProgramProviderGuid = new Guid(ProgramProviderString);
    }
}