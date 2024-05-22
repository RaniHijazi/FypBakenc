using Fyp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fyp.Interfaces
{
    public interface IDocumentRepository
    {
        Task UploadDocument(IFormFile image, int documentId);
        Task ApproveDocument(int documentId, int adminUserId);
        Task<List<Document>> DisplayDocumentsForAdmin(int adminUserId);
        Task RefuseDocument(int documentId, int adminUserId);
    }
}
