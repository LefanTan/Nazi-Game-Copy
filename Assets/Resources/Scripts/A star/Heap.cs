using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//optimizing pathfinding
public class Heap<T> where T : IHeapItem<T> {

	T[] items;
	int currentItemCount;

	public Heap(int maxHeapSize){
		items = new T[maxHeapSize];
	}

	//add it, then sort it up
	public void Add(T item){
		//add it at the end of the heap
		item.HeapIndex = currentItemCount;
		items [currentItemCount] = item;
		SortUp (item);
		currentItemCount++;
	}

	public T RemoveFirst(){
		T firstItem = items [0];
		currentItemCount--;

		//take last item on the heap and place it in the first
		items[0] = items[currentItemCount];
		items [0].HeapIndex = 0;

		SortDown (items [0]);
		return firstItem;
	}

	public bool Contains(T item){
		return Equals (items [item.HeapIndex], item);
	}

	public void UpdateItem(T item){
		SortUp (item);
	}

	public int Count{
		get{ 
			return currentItemCount;
		}
	}

	void SortUp(T item){
		int parentIndex = (item.HeapIndex - 1) / 2;

		while (true) {
			T parentItem = items [parentIndex];

			if (item.CompareTo (parentItem) > 0)
				Swap (item, parentItem);
			else
				break;
			
			parentIndex = (item.HeapIndex - 1) / 2;
		}

	}

	void SortDown(T item){

		while (true) {
			int childLeftIndex = (item.HeapIndex * 2) + 1;
			int childRightIndex = (item.HeapIndex * 2) + 2;
			int swapIndex = 0;

			//check if there's at least one child on the left
			if(childLeftIndex < currentItemCount){
				swapIndex = childLeftIndex;
				if (childRightIndex < currentItemCount) {
					//make right child the swap index if it has the higher priority
					if(items[childRightIndex].CompareTo(items[childLeftIndex]) > 0 ){
						swapIndex = childRightIndex;
					}
				}

				if (item.CompareTo (items[swapIndex]) < 0)
					Swap (item, items [swapIndex]);
				else
					return;

			}else{
				return;
			}
		}

	}

	void Swap(T itemA, T itemB){
		//swap actual item
		items[itemA.HeapIndex] = itemB;
		items [itemB.HeapIndex] = itemA;

		//swap indexes
		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}

}


public interface IHeapItem<T> : IComparable<T>{
	int HeapIndex{
		get;
		set;
	}
}