using Fyp.Dto;
using Fyp.Interfaces;
using Fyp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Fyp.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DataContext _context;
        private readonly BlobStorageService _blobStorageService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IFcmService _fcmService;

        public DocumentRepository(DataContext context, BlobStorageService blobStorageService, IHubContext<ChatHub> hubContext, IFcmService fcmService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
            _hubContext = hubContext;
            _fcmService = fcmService;

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
            var document = await _context.documents
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == documentId);


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

            Console.WriteLine($"Your Document {document.Name } Has been accepted");



            var notification = new Models.Notification
            {
                UserId = document.UserId,
                Content = $"{document.Name} Has been accepted",
                Time = DateTime.Now,
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();





            var connectionIds = ChatHub.ConnectedUsers.Where(kvp => kvp.Value == document.UserId.ToString()).Select(kvp => kvp.Key).ToList();
            if (connectionIds.Count > 0)
            {
                foreach (var connectionId in connectionIds)
                {
                    Console.WriteLine($"Sending notification to connection ID {connectionId}");
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", $"Your Document {document.Name} Has been accepted");
                }
                
            }
            else
            {
                Console.WriteLine($"User  is not connected.");
            }


            if (!string.IsNullOrEmpty(document.User.FcmToken))
            {
                await _fcmService.SendNotificationAsync(document.User.FcmToken, "Document Accepted", $"Your Document {document.Name} Has been accepted");
            }

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
            var document = await _context.documents
           .Include(d => d.User)
           .FirstOrDefaultAsync(d => d.Id == documentId);

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

            Console.WriteLine($"Your Document {document.Name} Has been refused,Please click on the document to view the reason");



            var notification = new Models.Notification
            {
                UserId = document.UserId,
                Content = $"{document.Name} Has been refused",
                Time = DateTime.Now,
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();





            var connectionIds = ChatHub.ConnectedUsers.Where(kvp => kvp.Value == document.UserId.ToString()).Select(kvp => kvp.Key).ToList();
            if (connectionIds.Count > 0)
            {
                foreach (var connectionId in connectionIds)
                {
                    Console.WriteLine($"Sending notification to connection ID {connectionId}");
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", $"Your Document {document.Name} Has been refused,Please click on the document to view the reason");
                }

            }
            else
            {
                Console.WriteLine($"User  is not connected.");
            }


            if (!string.IsNullOrEmpty(document.User.FcmToken))
            {
                await _fcmService.SendNotificationAsync(document.User.FcmToken, "Document Refused", $"Your Document {document.Name} Has been refused,Please click on the document to view the reason");
            }

        }
    }
}

