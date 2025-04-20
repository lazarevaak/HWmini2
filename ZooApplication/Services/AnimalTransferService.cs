using System;
using ZooApplication.DTOs;
using ZooApplication.Interfaces;
using ZooDomain;

namespace ZooApplication.Services
{
    public class AnimalTransferService : IAnimalTransferService
    {
        private readonly IAnimalRepository _animals;
        private readonly IEnclosureRepository _enclosures;

        public AnimalTransferService(
            IAnimalRepository animalRepository,
            IEnclosureRepository enclosureRepository)
        {
            _animals = animalRepository;
            _enclosures = enclosureRepository;
        }

        public async Task TransferAsync(TransferAnimalRequest request)
        {
            var animal = await _animals.GetByIdAsync(request.AnimalId)
                ?? throw new Excep($"Животное {request.AnimalId} не найдено.");
            var target = await _enclosures.GetByIdAsync(request.TargetEnclosureId)
                ?? throw new Excep($"Вольер {request.TargetEnclosureId} не найден.");

            // Проверка вместимости
            if (target.AnimalIds.Count >= target.Capacity)
                throw new Excep("Целевой вольер переполнен.");

            // Если у животного уже есть вольер — удаляем из старого
            if (animal.EnclosureId.HasValue)
            {
                var old = await _enclosures.GetByIdAsync(animal.EnclosureId.Value);
                if (old != null)
                {
                    old.RemoveAnimal(animal);
                    await _enclosures.UpdateAsync(old);
                }
            }

            // Перемещение внутри модели
            animal.MoveTo(target.Id);
            await _animals.UpdateAsync(animal);

            // Добавляем в новый вольер
            target.AddAnimal(animal);
            await _enclosures.UpdateAsync(target);
        }
    }
}

