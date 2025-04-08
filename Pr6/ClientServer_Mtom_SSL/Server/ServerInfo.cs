
using System;
using System.IO;

public class ServerInfo : IServerInfo
{
    public string GetPersonalisedServerName(string name)
    {
        return $"Cześć, {name}, tu serwer!";
    }

    public void UploadFile(FileUploadMessage request)
    {
        string uploadsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
        Directory.CreateDirectory(uploadsDir);
        string filePath = Path.Combine(uploadsDir, request.FileName);
        File.WriteAllBytes(filePath, request.FileData);
        Console.WriteLine("Plik zapisany: " + filePath);
    }
}
