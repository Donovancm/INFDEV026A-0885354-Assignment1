using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryPoint
{
    public class KDNode
    {
        //public bool IsEmpty { get; set; }
        public Vector2 vector2 { get; set; }
        public KDNode left { get; set; }
        public KDNode right { get; set; }
    }

    class Tree
    {
        public KDNode rootNode { get; set; }
        public List<Vector2> specialBuildingsInRange;

        public Tree()
        {
            rootNode = null;
            specialBuildingsInRange = new List<Vector2>();
        }


        public KDNode CreateTree(List<Vector2> elements, int depth)
        {
            if (elements.Count == 0)
            {
                return null;
            }

            List<Vector2> left = new List<Vector2>();
            List<Vector2> right = new List<Vector2>();

            //Sorting on depth = x
            if (depth % 2 == 0)
            {
                //Element in middle
                elements = elements.OrderBy(n => n.X).ToList();
                var middle = elements[elements.Count / 2]; 
                elements.Remove(middle);
                foreach (var item in elements)
                {
                    if (item.X <= middle.X)
                    {
                        left.Add(item);
                    }
                    else if (item.X > middle.X)
                    {
                        right.Add(item);
                    }
                }

                return new KDNode
                {
                    vector2 = middle,
                    left = CreateTree(left, depth + 1),
                    right = CreateTree(right, depth + 1)
                };
            }
            //Sorting on depth = y
            else
            {
                // Element in middle
                elements = elements.OrderBy(n => n.Y).ToList();
                var middle = elements[elements.Count / 2];
                elements.Remove(middle);
                foreach (var item in elements)
                {
                    if (item.Y <= middle.Y)
                    {
                        left.Add(item);
                    }
                    else if (item.Y > middle.Y)
                    {
                        right.Add(item);
                    }
                }

                return new KDNode
                {
                    vector2 = middle,
                    left = CreateTree(left, depth + 1),
                    right = CreateTree(right, depth + 1)
                };
            }
        }

        public void RangeSearch(Vector2 house, KDNode root, float maxDistance, int depth)
        {
            if (root == null)
            {
                return;
            }

            if (depth % 2 == 0)
            {
                if (root.vector2.X < (house.X - maxDistance))
                {
                    RangeSearch(house, root.right, maxDistance, depth + 1);
                }
                else if (root.vector2.X > (house.X + maxDistance))
                {
                    RangeSearch(house, root.left, maxDistance, depth + 1);
                }
                else if ((root.vector2.X >= (house.X - maxDistance)) && (root.vector2.X <= (house.X + maxDistance)))
                {
                    specialBuildingsInRange.Add(root.vector2);
                    RangeSearch(house, root.left, maxDistance, depth + 1);
                    RangeSearch(house, root.right, maxDistance, depth + 1);
                }
            }
            else
            {
                if (root.vector2.Y < (house.Y - maxDistance))
                {
                    RangeSearch(house, root.right, maxDistance, depth + 1);
                }
                else if (root.vector2.Y > (house.Y + maxDistance))
                {
                    RangeSearch(house, root.left, maxDistance, depth + 1);
                }
                else if ((root.vector2.Y >= (house.Y - maxDistance)) && (root.vector2.Y <= (house.Y + maxDistance)))
                {
                    specialBuildingsInRange.Add(root.vector2);
                    RangeSearch(house, root.left, maxDistance, depth + 1);
                    RangeSearch(house, root.right, maxDistance, depth + 1);
                }
            }
        }
        //    static void PrintPreOrder<T>(KDNode t)
        //    {
        //        if (t.IsEmpty) return;
        //        Console.WriteLine(t.Value);
        //        PrintPreOrder(t.Left);
        //        PrintPreOrder(t.Right);
        //    }

        //    static void PrintInOrder<T>(KDNode t)
        //    {
        //        if (t.IsEmpty) return;
        //        PrintInOrder(t.Left);
        //        Console.WriteLine(t.Value);
        //        PrintInOrder(t.Right);
        //    }
        //}
    }
}
