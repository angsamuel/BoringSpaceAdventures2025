using UnityEngine;
using System.Collections.Generic;

public enum BlockType
{
    NONE = 0,
    FLOOR = 1,
    WALL = 2
}

public struct Room
{
    public int pivotX;
    public int pivotY;
    public int width;
    public int height;
}

public class SlidingDungeonGenerator : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject floorPrefab;
    public int width;
    public int height;
    public float blockSize = 1f;

    private BlockType[,] blocks;
    private List<Vector2> floorPositions = new List<Vector2>();

    public void Start(){
        Generate(110);
    }
    public void Generate(uint seed1)
    {
        Random.InitState((int)seed1);
        ClearDungeon();

        Room initialRoom = new Room
        {
            pivotX = Random.Range(0, width - Random.Range(5, 8)),
            pivotY = Random.Range(0, height - Random.Range(5, 8)),
            width = Random.Range(5, 8),
            height = Random.Range(5, 8)
        };

        StampRoom(initialRoom);

        int attemptsLeft = 50;

        while (attemptsLeft > 0)
        {
            attemptsLeft--;

            Room tempRoom = new Room
            {
                width = Random.Range(3, 8),
                height = Random.Range(3, 8)
            };

            int spawnDirection = Random.Range(0, 4);
            switch (spawnDirection)
            {
                case 0:
                    tempRoom.pivotX = -tempRoom.width;
                    tempRoom.pivotY = Random.Range(-height, height);
                    break;
                case 1:
                    tempRoom.pivotX = width;
                    tempRoom.pivotY = Random.Range(-height, height);
                    break;
                case 2:
                    tempRoom.pivotY = -tempRoom.height;
                    tempRoom.pivotX = Random.Range(-width, width);
                    break;
                case 3:
                    tempRoom.pivotY = height;
                    tempRoom.pivotX = Random.Range(-width, width);
                    break;
            }

            Vector2 moveTo = floorPositions[Random.Range(0, floorPositions.Count)];

            int moveAttempts = Mathf.Max(width, height);
            while (moveAttempts > 0)
            {
                MoveRoom(ref tempRoom, moveTo);
                moveAttempts--;

                if (!RoomInBounds(tempRoom))
                {
                    continue;
                }
                if (BadOverlap(tempRoom))
                {
                    break;
                }
                if (AttemptStamp(tempRoom))
                {
                    break;
                }
            }
        }

        InstantiateBlocks();
    }

    private void ClearDungeon()
    {
        blocks = new BlockType[width, height];
        floorPositions.Clear();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InstantiateBlocks()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blocks[x, y] == BlockType.NONE) continue;

                Vector3 position = new Vector3(x * blockSize, 0, y * blockSize);
                GameObject prefab = blocks[x, y] == BlockType.FLOOR ? floorPrefab : blockPrefab;
                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }
    }

    private void PlaceBlock(BlockType block, int x, int y)
    {
        blocks[x, y] = block;

        if (block == BlockType.FLOOR)
        {
            floorPositions.Add(new Vector2(x, y));
        }
    }

    private BlockType GetBlock(int x, int y)
    {
        return blocks[x, y];
    }

    private void StampRoom(Room room)
    {
        for (int x = 0; x < room.width; x++)
        {
            for (int y = 0; y < room.height; y++)
            {
                if (x == 0 || y == 0 || x == room.width - 1 || y == room.height - 1)
                {
                    PlaceBlock(BlockType.WALL, x + room.pivotX, y + room.pivotY);
                }
                else
                {
                    PlaceBlock(BlockType.FLOOR, x + room.pivotX, y + room.pivotY);
                }
            }
        }
    }

    private void MoveRoom(ref Room room, Vector2 moveTo)
    {
        if (room.pivotX < moveTo.x)
        {
            room.pivotX += 1;
        }
        if (room.pivotX > moveTo.x)
        {
            room.pivotX -= 1;
        }
        if (room.pivotY < moveTo.y)
        {
            room.pivotY += 1;
        }
        if (room.pivotY > moveTo.y)
        {
            room.pivotY -= 1;
        }
    }

    private bool RoomInBounds(Room room)
    {
        if (room.pivotX < 0 || room.pivotY < 0)
        {
            return false;
        }
        if (room.pivotX + room.width >= width)
        {
            return false;
        }
        if (room.pivotY + room.height >= height)
        {
            return false;
        }
        return true;
    }

    private bool BadOverlap(Room room)
    {
        for (int x = 1; x < room.width - 1; x++)
        {
            for (int y = 1; y < room.height - 1; y++)
            {
                if (GetBlock(x + room.pivotX, y + room.pivotY) != BlockType.NONE)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool AttemptStamp(Room room)
    {
        for (int x = 0; x < room.width; x++)
        {
            for (int y = 0; y < room.height; y++)
            {
                if ((x == 0 || x == room.width - 1) && (y > 0 && y < room.height - 1))
                {
                    if (GetBlock(x + room.pivotX, y + room.pivotY) == BlockType.WALL)
                    {
                        if (GetBlock(x + room.pivotX - 1, y + room.pivotY) == BlockType.FLOOR || GetBlock(x + room.pivotX + 1, y + room.pivotY) == BlockType.FLOOR)
                        {
                            StampRoom(room);
                            PlaceBlock(BlockType.FLOOR, x + room.pivotX, y + room.pivotY);
                            return true;
                        }
                    }
                }
                if ((y == 0 || y == room.height - 1) && (x > 0 && x < room.width - 1))
                {
                    if (GetBlock(x + room.pivotX, y + room.pivotY) == BlockType.WALL)
                    {
                        if (GetBlock(x + room.pivotX, y + room.pivotY - 1) == BlockType.FLOOR || GetBlock(x + room.pivotX, y + room.pivotY + 1) == BlockType.FLOOR)
                        {
                            StampRoom(room);
                            PlaceBlock(BlockType.FLOOR, x + room.pivotX, y + room.pivotY);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
