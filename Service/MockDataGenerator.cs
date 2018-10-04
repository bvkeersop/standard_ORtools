using standard_ORtools.Model;
using System;
using System.Collections.Generic;

namespace standard_ORtools.Service
{
    class MockDataGeneratorService
    {
        private Random random;
        private int currentId;
        private const int minTravelTime = 1;
        private const int maxTravelTime = 5000;

        public MockDataGeneratorService()
        {
            currentId = 0;
            random = new Random();
        }
      
        private List<ClientTime> GenerateTravelTimeArray(int size)
        {
            int[] ttArray = new int[size];
            var travelTimeList = new List<ClientTime>();

            for (int i = 0; i < size; i++)
            {
                int travelTime = random.Next(minTravelTime, maxTravelTime);
                travelTimeList.Add(new ClientTime(i, travelTime));
            }

            return travelTimeList;
        }

        private int GetNextId()
        {
            return currentId++;
        }

        public List<Consultant> GenerateRandomConsultantList(int numberOfConsultants, int numberOfCompanies)
        {
            var consultantList = new List<Consultant>();

            for(int i = 0; i < numberOfConsultants; i++)
            {
                var consultant = new Consultant(GetNextId(), GenerateTravelTimeArray(numberOfCompanies));
                consultantList.Add(consultant);
            }

            return consultantList;
        }

    }
}
