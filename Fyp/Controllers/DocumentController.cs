using Fyp.Dto;
using Fyp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fyp.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] IFormFile image, [FromForm] int documentId)
        {
            try
            {
                await _documentRepository.UploadDocument(image, documentId);
                return Ok("Document uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading document: {ex.Message}");
            }
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveDocument([FromForm] int documentId, [FromForm] int adminUserId)
        {
            try
            {
                await _documentRepository.ApproveDocument(documentId, adminUserId);
                return Ok("Document approved successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error approving document: {ex.Message}");
            }
        }

        [HttpGet("adminDocuments")]
        public async Task<IActionResult> DisplayDocumentsForAdmin([FromQuery] int adminUserId)
        {
            try
            {
                var documents = await _documentRepository.DisplayDocumentsForAdmin(adminUserId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving documents for admin: {ex.Message}");
            }
        }

        [HttpPost("refuse")]
        public async Task<IActionResult> RefuseDocument([FromForm] int documentId, [FromForm] int adminUserId)
        {
            try
            {
                await _documentRepository.RefuseDocument(documentId, adminUserId);
                return Ok("Document refused successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error refusing document: {ex.Message}");
            }
        }
    }
}
