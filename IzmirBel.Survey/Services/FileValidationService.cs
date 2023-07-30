namespace IzmirBel.Survey.Services
{
    public class FileValidationService
    {
        private static readonly Dictionary<string, List<byte[]>> FileSignatures = new Dictionary<string, List<byte[]>>
        {
            {
                ".png", new List<byte[]>
                {
                    new byte[]
                    {
                        0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A
                    }
                }
            }
        };

        public bool IsValid(IFormFile uploadedFile)
        {
            string ext = Path.GetExtension(uploadedFile.FileName);

            if (!FileSignatures.ContainsKey(ext))
                return false; //file extension is wrong

            using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
            {
                var signatures = FileSignatures[ext];
                var headerBytes = binaryReader.ReadBytes(signatures.Max(x => x.Length));

                var result = signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));

                return result;
            }
        }
    }
}
