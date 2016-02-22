using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Heap{
    public List<Node> heap;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Heap()
    {
        heap = new List<Node>();
    }

    public Node RemoveRoot()
    {
        Node temp = heap[0];

        heap[0] = heap[heap.Count - 1];
        heap[heap.Count - 1] = temp;
        heap.RemoveAt(heap.Count - 1);
        SiftDown(0);
        return temp;
    }

    public void Insert(Node input, int f)
    {
        heap.Add(input);
        input.SetF(f);
        SiftUp(heap.Count - 1); //sift up last element
    }

    public bool isEmpty()
    {
        if(heap.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SiftUp(int nInd)
    {
        int parentInd = (nInd - 1) / 2;
        if (nInd == 0)
        {
            return;
        }
        if (heap[parentInd].F > heap[nInd].F)
        {
            Node temp = heap[parentInd];

            heap[parentInd] = heap[nInd];
            heap[nInd] = temp;
            SiftUp(parentInd);
        }
    }

    public void SiftDown(int nInd)
    {
        int leftChild = (2 * nInd) + 1;
        int rightChild = (2 * nInd) + 2;
        bool hasRight = false;
        int higherPriority;

        if (leftChild >= heap.Count)
        {
            return;
        }
        if (rightChild < heap.Count)
        {
            hasRight = true;
        }

        higherPriority = (hasRight && heap[rightChild].F < heap[leftChild].F ? rightChild : leftChild);
        if (heap[nInd].F > heap[higherPriority].F)
        {
            Node temp = heap[nInd];

            heap[nInd] = heap[higherPriority];
            heap[higherPriority] = temp;
            SiftDown(higherPriority);
        }
    }

    public int GetHeapSize()
    {
        return heap.Count;
    }
}