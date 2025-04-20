using System;
using ZooDomain;

namespace ZooPresentation.DTOs
{
    public record CreateEnclosureDto(
        EnclosureType Type,
        double Area,
        int Capacity
    );
}

