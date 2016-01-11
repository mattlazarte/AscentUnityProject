using UnityEngine;
using System.Collections;
using System.Text;

namespace Ascent.WaypointsSystem
{
	public class PriorityQueue
	{
		public class Node
		{
            public Node(RuntimeWaypoint wp, float priority)
			{
				this.priority = priority;
				this.waypoint = wp;
			}

			public float priority;
            public RuntimeWaypoint waypoint;
			public Node next;
		}

		private Node topNode;

		public bool IsNotEmpty
		{
			get { return topNode != null; }
		}

        public void Enqueue(RuntimeWaypoint wp, float priority)
		{
			var newNode = new Node(wp, priority);

			if (topNode == null)
			{
				topNode = newNode;
			}
			else
			{
				Node previousNode = null;
				Node currentNode = topNode;
				var currentNodeIsTop = true;
				var inserted = false;

				while (currentNode != null)
				{
					if (currentNode.priority > priority)
					{
						newNode.next = currentNode;

						if (currentNodeIsTop)
						{
							topNode = newNode;
						}
						else
						{
							previousNode.next = newNode;
						}

						inserted = true;
						break;
					}

					previousNode = currentNode;
					currentNode = currentNode.next;
					currentNodeIsTop = false;
				}

				if (!inserted)
				{
					previousNode.next = newNode;
				}
			}
		}

        public RuntimeWaypoint Dequeue()
		{
			if (topNode == null)
			{
				return null;
			}

			var nodeToReturn = topNode;
			topNode = nodeToReturn.next;
			
			return nodeToReturn.waypoint;
		}
	}
}