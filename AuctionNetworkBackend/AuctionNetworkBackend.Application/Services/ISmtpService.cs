using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Services
{
    /// <summary>
    /// Service used for email communication.
    /// </summary>
    public interface ISmtpService
    {
        /// <summary>
        /// Sends message.
        /// </summary>
        /// <param name="email">Recipient's email.</param>
        /// <param name="subject">Subject of email.</param>
        /// <param name="message">Message.</param>
        /// <returns></returns>
        Task SendMessage(string email, string subject, string message);
    }
}