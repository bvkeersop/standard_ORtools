using standard_ORtools.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace standard_ORtools.Service
{
    class MockDataGenerator
    {
        private Random random;
        private int currentId;
        private const int maxTravelTime = 120;

        public MockDataGenerator()
        {
            currentId = 0;
            random = new Random();
        }
      
        private int[] GenerateTravelTimeArray(int size)
        {
            int[] ttArray = new int[size];

            for(int i = 0; i < size; i++)
            {
                ttArray[i] = random.Next(maxTravelTime);
            }

            return ttArray;
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
