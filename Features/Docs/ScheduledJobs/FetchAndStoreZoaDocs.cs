﻿using Coravel.Invocable;
using Microsoft.Extensions.Options;
using ZmaReference.Features.Docs.Models;
using ZmaReference.Features.Docs.Repositories;

namespace ZmaReference.Features.Docs.ScheduledJobs;

public class FetchAndStoreZmaDocs(ILogger<FetchAndStoreZmaDocs> logger, IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment, IOptionsMonitor<AppSettings> appSettings, DocumentRepository documentRepository) : IInvocable
{
    public async Task Invoke()
    {
        var client = httpClientFactory.CreateClient();
        List<DocumentCategory>? fetchedDocCategories = null;

        try
        {
            logger.LogInformation("Fetching ZMA docs from {url}",
                appSettings.CurrentValue.Urls.ZmaDocumentsApiEndpoint);
            fetchedDocCategories = await client.GetFromJsonAsync<List<DocumentCategory>>(appSettings.CurrentValue.Urls.ZmaDocumentsApiEndpoint);
            logger.LogInformation("Successfully fetched ZMA docs");
        }
        catch (Exception e)
        {
            logger.LogError("Error while fetching ZMA docs: {ex}", e.ToString());
        }

        if (fetchedDocCategories is null || fetchedDocCategories.Count == 0)
        {
            logger.LogInformation("Fetched ZMA documents null or zero");
            return;
        }

        var tasks = new List<Task>();
        foreach (var category in fetchedDocCategories)
        {
            foreach (var doc in category.Documents)
            {
                var pdfName = GetPdfNameFromUrl(doc.Url);
                var localPdfPath = Path.ChangeExtension(Path.Combine(PdfFolderPath, pdfName), ".pdf");
                if (!File.Exists(localPdfPath))
                {
                    var task = WriteRemotePdfToLocal(doc.Url, localPdfPath);
                    tasks.Add(task);
                }
                else
                {
                    logger.LogInformation("Found pdf at {url} but file already exists at {path}", doc.Url, localPdfPath);
                }
            }
        }
        
        await Task.WhenAll(tasks);
        documentRepository.ClearAllDocumentCategories();
        documentRepository.AddDocumentCategories(fetchedDocCategories);
    }
    
    private string PdfFolderPath => Path.Combine(webHostEnvironment.WebRootPath, appSettings.CurrentValue.DocumentsPdfPath);

    private static string GetPdfNameFromUrl(string url)
    {
        var uri = new Uri(url);
        return Path.GetFileName(uri.AbsolutePath);
    }

    private async Task WriteRemotePdfToLocal(string url, string path)
    {
        var client = httpClientFactory.CreateClient();
        await using var pdfStream = await client.GetStreamAsync(url);

        var dirPath = Path.GetDirectoryName(path);
        if (dirPath is not null && !Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        
        await using var pdfNewFile = File.Create(path);
        await pdfStream.CopyToAsync(pdfNewFile);
        logger.LogInformation("Wrote new PDF to {path}", path);
    }
}