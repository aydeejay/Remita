using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Remita.Service.Models;

namespace Remita.Service.Interfaces
{
    public interface IHttpService
    {
        Task<RRRResponse> GenerateRRR(RRRRequest rRRRequest);
    }
}