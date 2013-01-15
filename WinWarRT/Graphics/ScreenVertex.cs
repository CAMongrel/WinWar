// Author: Henning
// Project: WinWarEngine
// Path: P:\Projekte\WinWarCS\WinWarEngine\Graphics
// Creation date: 18.11.2009 10:13
// Last modified: 27.11.2009 10:14

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WinWarRT.Graphics
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ScreenVertex
	{
		[FieldOffset(0)]
		public Vector2 Position;

		[FieldOffset(8)]
		public Vector2 UV;

		[FieldOffset(16)]
		public Vector4 Color;

		public static VertexDeclaration VertexDecl = 
			CreateVertexDeclaration();

		private static VertexDeclaration CreateVertexDeclaration()
		{
            throw new NotImplementedException();
			/*return new VertexDeclaration(BaseGame.device,
				new VertexElement[] {
						new VertexElement(0, 0, VertexElementFormat.Vector2,
							VertexElementMethod.Default, VertexElementUsage.Position, 0),

						new VertexElement(0, 8, VertexElementFormat.Vector2,
							VertexElementMethod.Default, 
							VertexElementUsage.TextureCoordinate, 0),

							new VertexElement(0, 16, VertexElementFormat.Vector4,
							VertexElementMethod.Default, 
							VertexElementUsage.Color, 0)
					});
            */
		}

		public const int SizeOf = 4 * (2 + 2 + 4);
	}
}
