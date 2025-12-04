using Godot;
using System.Collections.Generic;

public partial class Map : Node2D
{
	[Export] private Node2D MapChunk;
	[Export] private Camera2D Camera;

	// Chunk despawn distance
	[Export] private float despawnDistance = 3000f;

	// All active chunks tracked
	private Dictionary<Vector2I, Node2D> chunks = new();

	// Size of each chunk in world
	[Export] private Vector2 chunkSize = new Vector2(1152, 672);

	public override void _Ready()
	{
		if(MapChunk == null)
		{
			GD.Print("Assign Map Chunk in parent node!");
		}
		if(Camera == null)
		{
			GD.Print("Assign Camera in parent node!");
		}
	}
	
	public override void _Process(double delta)
	{
		UpdateChunksAroundCamera();
		DespawnFarChunks();
	}

	private void UpdateChunksAroundCamera()
	{
		Vector2 camPos = Camera.GlobalPosition;

		// Determine current visible chunks
		Vector2I camChunkCoord = new(
			Mathf.FloorToInt(camPos.X / chunkSize.X),
			Mathf.FloorToInt(camPos.Y / chunkSize.Y)
		);

		// Spawn all surrounding chunks
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				Vector2I coord = new(camChunkCoord.X + x, camChunkCoord.Y + y);
				AddChunk(coord);
			}
		}
	}

	// Spawn individual chunk 
	private void AddChunk(Vector2I coord)
	{
		if (chunks.ContainsKey(coord))
			return;

		var chunk = (Node2D)MapChunk.Duplicate();
		chunk.ZIndex = -1;
		AddChild(chunk);

		Vector2 pos = new Vector2(
			coord.X * chunkSize.X,
			coord.Y * chunkSize.Y
		);
		chunk.GlobalPosition = pos;

		chunks.Add(coord, chunk);
	}

	// Despawn chunks out of view
	private void DespawnFarChunks()
	{
		Vector2 camPos = Camera.GlobalPosition;

		List<Vector2I> toRemove = new();

		foreach (var entry in chunks)
		{
			Node2D chunk = entry.Value;

			float dist = camPos.DistanceTo(chunk.GlobalPosition);

			if (dist > despawnDistance)
				toRemove.Add(entry.Key);
		}

		foreach (var key in toRemove)
		{
			chunks[key].QueueFree();
			chunks.Remove(key);
		}
	}

	// Calculate boundaries of individual chunks
	private Vector2 CalculateChunkBounds(Node2D chunk)
	{
		Rect2 totalRect = new Rect2(Vector2.Inf, -Vector2.Inf);

		foreach (Node child in chunk.GetChildren())
		{
			if (child is TileMapLayer map)
			{
				Rect2 used = map.GetUsedRect();
				Vector2 tileSize = map.TileSet.TileSize;

				Vector2 mapPxSize = used.Size * tileSize;

				Rect2 pixelRect = new Rect2(used.Position * tileSize, mapPxSize);

				totalRect = totalRect.Encloses(pixelRect) ? totalRect : totalRect.Merge(pixelRect);
			}
		}

		return totalRect.Size;
	}
}
