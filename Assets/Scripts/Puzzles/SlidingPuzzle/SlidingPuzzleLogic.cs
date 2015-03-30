/* Copyright (c) 2015 ETH Zurich, Tizian Zeltner
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
 
using UnityEngine;
using System.Collections.Generic;

public class SlidingPuzzleLogic : PuzzleLogic
{
    public List<GameObject> tiles;

	private int size = 3;

	[HideInInspector]
	public float z = 0.02849352f;

	[HideInInspector]
	public float delta = 0.1755742f;

	private int[,] internTiles;

	public int MoveTile(int k)
	{
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				if (internTiles[i,j] == k)
				{
					// Check right
					if (j < size-1)
					{
						if (internTiles[i,j+1] == 0)
						{
							internTiles[i,j+1] = k;
							internTiles[i,j] = 0;
							return 1;
						}
					}
					// Check left
					if (j > 0)
					{
						if (internTiles[i,j-1] == 0)
						{
							internTiles[i,j-1] = k;
							internTiles[i,j] = 0;
							return -1;
						}
					}
					// Check down
					if (i < size-1)
					{
						if (internTiles[i+1,j] == 0)
						{
							internTiles[i+1,j] = k;
							internTiles[i,j] = 0;
							return 2;
						}
					}
					// Check up
					if (i > 0)
					{
						if (internTiles[i-1,j] == 0)
						{
							internTiles[i-1,j] = k;
							internTiles[i,j] = 0;
							return -2;
						}
					}
					return 0;
				}
			}
		}
		return 0;
	}

    override protected void Reset()
    {
        // Not all random puzzles are solvable.
        // So we create a solved puzzle and do actual moves to end up with a random (but solvable) puzzle.
		internTiles = new int[size, size];
        int k = 1;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
				internTiles[y, x] = k;
                k++;
            }
        }
		internTiles[size - 1, size - 1] = 0;
        int emptyX = size - 1;
        int emptyY = size - 1;
        
        k = 1;
        while (k < 200)
        {
            int delta = Random.Range(-1, 2);
            int dir = Random.Range(-1, 2);
            int newX, newY;
            
            if (dir == -1)
            {
                newX = emptyX + delta;
                newY = emptyY;
            }
            else
            {
                newX = emptyX;
                newY = emptyY + delta;
            }
            
            if (0 <= newX && newX < size && 0 <= newY && newY < size)
            {
				internTiles[emptyY, emptyX] = internTiles[newY, newX];
				internTiles[newY, newX] = 0;
                emptyX = newX;
                emptyY = newY;
                k = k + 1;
            }
        }
        
        // Go through all tiles and set their position according to the shuffled positions.
        int i = 0;
		for (float y = delta; y >= -delta; y = y - delta)
        {
            int j = 0;
			for (float x = delta; x >= -delta; x = x - delta)
            {
				k = internTiles[i, j];
                if (k < size * size)
                {
					if (k != 0)
					{
						tiles[k - 1].transform.localPosition = new Vector3(x, y, z);
						tiles[k - 1].GetComponent<TileMovement>().Activate();
					}
					j = j + 1;
                }
            }
			i = i + 1;
        }
    }

    override protected bool CheckIfSolved()
    {
		bool solved = true;
		
		int k = 1;
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				if (k < size*size)
				{
					if (internTiles[i, j] != k)
					{
						solved = false;
					}
				}
				k = k + 1;
			}
		}

		return solved;
    }

	override protected void OnSkip()
    {
        int k = 0;
        for (float y = delta; y >= -delta; y = y - delta)
        {
            for (float x = delta; x >= -delta; x = x - delta)
            {
				if (k < size*size-1)
				{
	                tiles[k].transform.localPosition = new Vector3(x, y, z);
					tiles[k].GetComponent<TileMovement>().Deactivate();
				}
                k = k + 1;
            }
        }
    }

    override protected void OnSolve()
    {
		foreach (GameObject tile in tiles)
		{
			tile.GetComponent<TileMovement>().Deactivate();
		}
    }
}