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

        public async Task UploadDocument(IFormFile image, int documentId)
        {
            var document = await _context.documents.FindAsync(documentId);

            string imgUrl = await _blobStorageService.UploadImageAsync(image);

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

        public async Task<List<Document>> DisplayDocumentsForAdmin(int adminUserId)
        {

            var documents = await _context.documents
                .Where(d => d.Status == "Pending" || d.Status == "Refused")
                .ToListAsync();

            return documents;
        }

        public async Task RefuseDocument(int documentId, int adminUserId)
        {
            var document = await _context.documents.FindAsync(documentId);

            if (document == null)
            {
                throw new InvalidOperationException("Document not found");
            }


            document.Status = "Refused";


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

