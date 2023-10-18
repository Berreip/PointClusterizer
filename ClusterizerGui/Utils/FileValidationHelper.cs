using System.Diagnostics.CodeAnalysis;
using System.IO;
using PRF.Utils.CoreComponents.IO;

namespace ClusterizerGui.Utils
{
    public static class FileValidationHelper
    {
        public static bool TryValidateFileInput(string? file, [NotNullWhen(returnValue: true)] out IFileInfo? validFile)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(file) && File.Exists(file))
                {
                    // try to create the file
                    validFile = new FileInfoWrapper(file);
                    return true;
                }
            }
            catch
            {
                // ignore and return false
            }
            validFile = null;
            return false;
        }
    }
}