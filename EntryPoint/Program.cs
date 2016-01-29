using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryPoint
{
#if WINDOWS || LINUX
  public static class Program
  {
    [STAThread]
    static void Main()
    {
      var fullscreen = false;
      read_input:
      switch (Microsoft.VisualBasic.Interaction.InputBox("Which assignment shall run next? (1, 2, 3, 4, or q for quit)", "Choose assignment", VirtualCity.GetInitialValue()))
      {
        case "1":
          using (var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen))
            game.Run();
          break;
        case "2":
          using (var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen))
            game.Run();
          break;
        case "3":
          using (var game = VirtualCity.RunAssignment3(FindRoute, fullscreen))
            game.Run();
          break;
        case "4":
          using (var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen))
            game.Run();
          break;
        case "q":
          return;
      }
      goto read_input;
    }       

                                                    //Assignment1\\
        //Using the MergeSort structure
        //Following slide 29 for example
        private static Vector2[] Mergesort(Vector2[] Vector2Array, int StartValue, int EndValue, Vector2 house)
        {
            Vector2[] newarray = new Vector2[1];
            if (StartValue < EndValue)
            {
                int half = (StartValue + EndValue) / 2;
                Mergesort(Vector2Array, StartValue, half, house);
                Mergesort(Vector2Array, half + 1, EndValue, house);
                newarray = Merge(Vector2Array, StartValue, half, EndValue, house);
            }
            return newarray;
        }
        //Working in progress
        //following slide 35 for example
        private static Vector2[] Merge(Vector2[] Vector2Array, int start, int half, int end, Vector2 house)
        {
            int leftArrayContent  = half - start + 1;
            int rightArrayContent = end - half;

            Vector2[] leftarray  = new Vector2[leftArrayContent + 1];
            Vector2[] rightarray = new Vector2[rightArrayContent + 1];

            int i = 0;
            int j = 0;

            for(i =0; i < leftArrayContent; i++ )
            {
                leftarray[i] = Vector2Array[start + i];
            }
            for(j = 0; j < rightArrayContent; j++)
            {
                rightarray[j] = Vector2Array[half + 1 + j];
            }

            // adding infinity integer for x and y?
            leftarray[leftArrayContent] = new Vector2(float.MaxValue, float.MaxValue);
            rightarray[rightArrayContent] = new Vector2(float.MaxValue, float.MaxValue);

            i = 0;
            j = 0;
            double leftdifference  = EuclDistance(house, leftarray[i]);
            double rightdifference = EuclDistance(house, rightarray[j]);

            for(int k = start; k <= end; k++)
            {
                if( leftdifference <= rightdifference)
                {
                    Vector2Array[k] = leftarray[i];
                    i++;
                    leftdifference = EuclDistance(house, leftarray[i]);
                }
                else
                {
                    Vector2Array[k] = rightarray[j];
                    j++;
                    rightdifference = EuclDistance(house, rightarray[j]);
                } 
            }
            return Vector2Array;
            
        }
            //Trying to read the values the values in vector2 and specialbuildings
            private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
            {

                Console.WriteLine(specialBuildings);
                Vector2[] specialbuildingsarray = specialBuildings.ToArray();
                int first = 0;
                int last = specialBuildings.Count() - 1;

                Vector2[] sortedhouses = Mergesort(specialbuildingsarray, first, last, house);
                return sortedhouses;
                
                //Console.WriteLine("houseY"+ "houseY");
                //Console.WriteLine(houseX);
                //result = Math.Round(Math.Sqrt(Math.Pow(houseX - buildingX, 2) + Math.Pow(houseY - buildingY, 2)));
                //Console.WriteLine(result);
                //return specialBuildings.OrderBy(v => Vector2.Distance(v, house));
            }
            //Defining the Euclidean Distance for this assignment
            private static double EuclDistance(Vector2 specialbuilding, Vector2 house)
            {
                float deltaX = house.X - specialbuilding.X;
                float deltaY = house.Y - specialbuilding.Y;
                float result = (deltaX * deltaX) + (deltaY * deltaY);
              
                //double result = Math.Round(Math.Sqrt(Math.Pow(houseX - buildingX) 2) + Math.Pow(houseY - buildingY, 2)));
                return result;
                //float buildingX = element.X;
                //float buildingY = element.Y;      
            }

                                                    //Assignment 1\\
                                                    //Assignment 2\\
                                                                                        

        private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(IEnumerable<Vector2> specialBuildings, IEnumerable<Tuple<Vector2, float>> housesAndDistances)
        {
            List<List<Vector2>> specialBuildingsInRange = new List<List<Vector2>>();
            List<Vector2> specialBuildingsInCircle = new List<Vector2>();
            List<Tuple<Vector2, float>> buildingList = housesAndDistances.ToList();

            var tree = new Tree();
            var create = tree.CreateTree(specialBuildings.ToList(), 0);

            foreach (var building in buildingList)
            {
                tree.RangeSearch(building.Item1, create, building.Item2, 0);

                foreach (var x in tree.specialBuildingsInRange)
                {
                    if (Vector2.Distance(building.Item1, x) <= building.Item2)
                    {
                        specialBuildingsInCircle.Add(x);
                        
                    }
                }
            }

            specialBuildingsInRange.Add(specialBuildingsInCircle);

            return specialBuildingsInRange;
        }
        
                                                    //Assignment 2\\
                                                    //Assignment 3\\
        private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
          Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
        {
          var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
          List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
          var prevRoad = startingRoad;
          for (int i = 0; i < 30; i++)
          {
            prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
            fakeBestPath.Add(prevRoad);
          }
          return fakeBestPath;
        }
                                                        //Assignment 3\\
        private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding, 
          IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
        {
          List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
          foreach (var d in destinationBuildings)
          {
            var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
            List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
            var prevRoad = startingRoad;
            for (int i = 0; i < 30; i++)
            {
              prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
              fakeBestPath.Add(prevRoad);
            }
            result.Add(fakeBestPath);
          }
          return result;
        }
  }
#endif
}
