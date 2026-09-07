using System;

namespace ValheimPlus.Utility
{
    /// <summary>Reporting for patches that could not be applied.</summary>
    public static class PatchLog
    {
        /// <summary>
        /// Reports that a patch did not apply, so whatever it enables will not work.
        /// <paramref name="detail"/> should say what the user loses.
        /// </summary>
        public static void Failed(string patch, string detail = null, Exception exception = null)
        {
            var message = $"Failed to apply `{patch}`.";
            if (detail != null) message += " " + detail;
            if (exception != null) message += $" Exception is:\n{exception}";
            ValheimPlusPlugin.Logger.LogError(message);
        }
    }
}
