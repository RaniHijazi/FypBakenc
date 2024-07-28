using Fyp.Dto;
using Fyp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fyp.Interfaces
{
    public interface IDocumentRepository
    {
        Task UploadDocument(int documentId, IFormFile image);
        Task ApproveDocument(int documentId, int adminUserId);
        Task<List<DocumentDto>> DisplayDocumentsForAdmin(int adminUserId);
        Task RefuseDocument(int documentId, int adminUserId, string note);
        Task<List<Document>> DisplayUserDocuments(int userId);
    }
}
