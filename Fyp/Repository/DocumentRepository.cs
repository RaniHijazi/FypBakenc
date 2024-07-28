using Fyp.Dto;
using Fyp.Interfaces;
using Fyp.Models;
using Microsoft.EntityFrameworkCore;

namespace Fyp.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DataContext _context;
        private readonly BlobStorageService _blobStorageService;

        public DocumentRepository(DataContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task UploadDocument( int documentId,IFormFile image)
        {
            var document = await _context.documents.FindAsync(documentId);

            string imgUrl = await _blobStorageService.UploadImageAsync(image);
            document.Status = "Uploaded";
            document.ImgUrl = imgUrl;
            await _context.SaveChangesAsync();


        }
        public async Task ApproveDocument(int documentId, int adminUserId)
        {
            var document = await _context.documents.FindAsync(documentId);

            if (document == null)
            {
                throw new InvalidOperationException("Document not found");
            }


            document.Status = "Approved";


            var approval = await _context.documents_approval.FirstOrDefaultAsync(da => da.DocumentId == documentId);
            if (approval == null)
            {
                approval = new DocumentApproval
                {
                    DocumentId = documentId,
                    ApprovedById = adminUserId,
                    ApprovalDate = DateTime.Now,
                    Status = "Approved"
                };
                _context.documents_approval.Add(approval);
            }
            else
            {
                approval.ApprovedById = adminUserId;
                approval.ApprovalDate = DateTime.Now;
                approval.Status = "Approved";
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<DocumentDto>> DisplayDocumentsForAdmin(int adminUserId)
        {
            var documents = await _context.documents
                .Where(d => d.Status == "Uploaded")
                .Select(d => new DocumentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    ImgUrl = d.ImgUrl,
                    username = d.User.FullName,
                    UploadDate=d.UploadDate,
                })
                .ToListAsync();

            return documents;
        }


        public async Task<List<Document>> DisplayUserDocuments(int userId)
        {
            var documents = await _context.documents
                .Where(d => d.UserId == userId)
                .ToListAsync();
            return documents;
        }


        public async Task RefuseDocument(int documentId, int adminUserId,string note)
        {
            var document = await _context.documents.FindAsync(documentId);

            if (document == null)
            {
                throw new InvalidOperationException("Document not found");
            }


            document.Status = "Refused";
            document.Note = note;


            var approval = await _context.documents_approval.FirstOrDefaultAsync(da => da.DocumentId == documentId);
            if (approval == null)
            {
                approval = new DocumentApproval
                {
                    DocumentId = documentId,
                    ApprovedById = adminUserId,
                    ApprovalDate = DateTime.Now,
                    Status = "Refused"
                };
                _context.documents_approval.Add(approval);
            }
            else
            {
                approval.ApprovedById = adminUserId;
                approval.ApprovalDate = DateTime.Now;
                approval.Status = "Refused";
            }

            await _context.SaveChangesAsync();
        }
    }
}

