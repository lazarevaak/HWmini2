using System;
using ZooApplication.DTOs;

namespace ZooApplication.Services
{
    public interface IAnimalTransferService
    {
        Task TransferAsync(TransferAnimalRequest request);
    }
}

