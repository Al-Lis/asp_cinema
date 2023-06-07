using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectLibrary.Models;

namespace pitpm_pr1.Models
{
    public class TicketsModel
    {
             public int SessionId { get; set; }

             public int RowNumber { get; set; }

             public int SeatNumber { get; set; }

             public int UserId { get; set; }
 
    }
}
