using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PolyScript
{

    public class CarsManager : MonoBehaviour
    {
        [SerializeField] private List<Vehicle> cars;
        [SerializeField] private GameEvent OnCarChanged;
        [SerializeField] private List<Texture> carTextures;
        private int activeCarIndex, activeTextureIndex;

        private void OnEnable()
        {
            activeTextureIndex = 0;
            activeCarIndex = 0;
            HideAllCars();
            ActivateCar(activeCarIndex);
            SetTexture(activeTextureIndex);
        }

        public void SetPreviousTexture() => SelectTexture(false);

        public void SetNextTexture() => SelectTexture(true);

        private void SetTexture(int index)
        {
            cars[activeCarIndex].SetTexture(carTextures[index]);
            activeTextureIndex = index;
        }

        private void SelectTexture(bool isNextButtonClicked)
        {
            var newindex = GetNewIndex(activeTextureIndex, carTextures.Count, isNextButtonClicked);
            SetTexture(newindex);
        }

        public void SelectPreviousCar()
        {
            SelectCar(false);
        }
        public void SelectNextCar()
        {
            SelectCar(true);
        }
        private void ActivateCar(int newIndex)
        {

            cars[activeCarIndex].gameObject.SetActive(false);
            activeCarIndex = newIndex;
            cars[activeCarIndex].gameObject.SetActive(true);
        }
        private void SelectCar(bool isNextButtonClicked)
        {
            var newindex = GetNewIndex(activeCarIndex, cars.Count, isNextButtonClicked);
            ActivateCar(newindex);
            OnCarChanged?.Raise();
        }

        private int GetNewIndex(int index, int sizeOfList, bool next)
        {
            if (next)
            {
                return (index + 1) % sizeOfList;
            }
            else
            {
                return index == 0 ? sizeOfList - 1 : index - 1;
            }
        }

        private void HideAllCars()
        {
            foreach (Vehicle car in cars)
            {
                car.gameObject.SetActive(false);
            }
        }


    }
}

