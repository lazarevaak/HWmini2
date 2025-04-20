using System;
namespace ZooPresentation.DTOs
{
    public record CreateAnimalDto(
        string Species,
        string Name,
        DateTime DateOfBirth,
        string Gender,
        string FavoriteFoodName,
        int FavoriteFoodCalories
    );
}

