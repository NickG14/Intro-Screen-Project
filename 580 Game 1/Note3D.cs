using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace SwordsDance
{
    class Note3D
    {
        /// <summary>
        /// The vertices of the cube
        /// </summary>
        VertexBuffer vertices;

        /// <summary>
        /// The vertex indices of the cube
        /// </summary>
        IndexBuffer indices;

        /// <summary>
        /// The effect to use rendering the cube
        /// </summary>
        BasicEffect effect;

        /// <summary>
        /// The game this cube belongs to 
        /// </summary>
        Game game;

        /// <summary>
        /// Used to calculate the position of the note
        /// </summary>
        Vector3 position;
        Vector3 velocity;

        float updatePosition = -100f;

        public bool hit = false;

        public bool stopped = true;

        private float leftShrink = -1;
        private float rightShrink = 1;

        private VertexPositionColor[] vertexData;

        int lane;

        /// <summary>
        /// Constructs a cube instance
        /// </summary>
        /// <param name="game">The game that is creating the cube</param>
        public Note3D(MainWindow game, int random)
        {
            this.game = game;
            InitializeVertices(random);
            InitializeIndices();
            InitializeEffect();
            position = new Vector3(0, 30, 0);
            velocity = new Vector3(0, -5, 0);

            if(random == 0)
            {
                lane = -8;
            }
            else
            {
                lane = 8;
            }
        }

        /// <summary>
        /// Initialize the vertex buffer
        /// </summary>
        public void InitializeVertices(int random)
        {
            if (random == 0)
            {
                vertexData = new VertexPositionColor[] {
                    new VertexPositionColor() { Position = new Vector3(-1, 1, -3), Color = Color.LightPink },
                    new VertexPositionColor() { Position = new Vector3(1, 1, -3), Color = Color.LightPink },
                    new VertexPositionColor() { Position = new Vector3(-1, -1, -3), Color = Color.DarkRed },
                    new VertexPositionColor() { Position = new Vector3(1, -1, -3), Color = Color.DarkRed },
                    new VertexPositionColor() { Position = new Vector3(-1, 1, 3), Color = Color.LightPink },
                    new VertexPositionColor() { Position = new Vector3(1, 1, 3), Color = Color.LightPink },
                    new VertexPositionColor() { Position = new Vector3(-1, -1, 3), Color = Color.DarkRed },
                    new VertexPositionColor() { Position = new Vector3(1, -1, 3), Color = Color.DarkRed }
                };
            }
            else
            {
                vertexData = new VertexPositionColor[] {
                        new VertexPositionColor() { Position = new Vector3(-1, 1, -3), Color = Color.LightBlue },
                        new VertexPositionColor() { Position = new Vector3(1, 1, -3), Color = Color.LightBlue },
                        new VertexPositionColor() { Position = new Vector3(-1, -1, -3), Color = Color.DarkBlue },
                        new VertexPositionColor() { Position = new Vector3(1, -1, -3), Color = Color.DarkBlue },
                        new VertexPositionColor() { Position = new Vector3(-1, 1, 3), Color = Color.LightBlue },
                        new VertexPositionColor() { Position = new Vector3(1, 1, 3), Color = Color.LightBlue },
                        new VertexPositionColor() { Position = new Vector3(-1, -1, 3), Color = Color.DarkBlue },
                        new VertexPositionColor() { Position = new Vector3(1, -1, 3), Color = Color.DarkBlue }
                    };
            }

            
            vertices = new VertexBuffer(
                game.GraphicsDevice,            // The graphics device to load the buffer on 
                typeof(VertexPositionColor),    // The type of the vertex data 
                8,                              // The count of the vertices 
                BufferUsage.None                // How the buffer will be used
            );
            vertices.SetData<VertexPositionColor>(vertexData);
        }

        /// <summary>
        /// Initializes the index buffer
        /// </summary>
        public void InitializeIndices()
        {
            var indexData = new short[]
            {
            0, 1, 2, // Side 0
            2, 1, 3,
            4, 0, 6, // Side 1
            6, 0, 2,
            7, 5, 6, // Side 2
            6, 5, 4,
            3, 1, 7, // Side 3 
            7, 1, 5,
            4, 5, 0, // Side 4 
            0, 5, 1,
            3, 7, 2, // Side 5 
            2, 7, 6
            };
            indices = new IndexBuffer(
                game.GraphicsDevice,            // The graphics device to use
                IndexElementSize.SixteenBits,   // The size of the index 
                36,                             // The count of the indices
                BufferUsage.None                // How the buffer will be used
            );
            indices.SetData<short>(indexData);
        }

        /// <summary>
        /// Initializes the BasicEffect to render our cube
        /// </summary>
        void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(
                new Vector3(0, 0, 4), // The camera position
                new Vector3(0, 0, 0), // The camera target,
                Vector3.Up            // The camera up vector
            );
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                1.3f,                         // The field-of-view 
                game.GraphicsDevice.Viewport.AspectRatio,   // The aspect ratio
                0.1f, // The near plane distance 
                100.0f // The far plane distance
            );
            effect.VertexColorEnabled = true;
            
        }

        /// <summary>
        /// Updates the Cube
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if(!hit && !stopped)
            {
                updatePosition += 0.41f;                         
                // Look at the cube from farther away while spinning around it

                Matrix movement = Matrix.CreateTranslation(new Vector3(lane, -10, 0));
                effect.World = Matrix.CreateLookAt(
                    new Vector3(updatePosition, 0, 0),
                    Vector3.Zero,
                    Vector3.Up
                ) * movement;
                effect.Alpha = 1;
            }

            else if (hit || leftShrink < 0.1)
            {
                leftShrink += .02f;
                rightShrink -= .02f;
                effect.Alpha = rightShrink;
                vertexData[0].Color = Color.Red;
                vertexData[1].Color = Color.Red;
                vertexData[2].Color = Color.Red;

            }
            else 
            { 
                updatePosition = -100f;
                leftShrink = -1;
                rightShrink = 1;
            }


        }

        /// <summary>
        /// Draws the Cube
        /// </summary>
        public void Draw()
        {
            // apply the effect 
            effect.CurrentTechnique.Passes[0].Apply();
            // set the vertex buffer
            game.GraphicsDevice.SetVertexBuffer(vertices);
            // set the index buffer
            game.GraphicsDevice.Indices = indices;
            // Draw the triangles
            game.GraphicsDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList, // Tye type to draw
                0,                          // The first vertex to use
                0,                          // The first index to use
                12                       // the number of triangles to draw
            );

        }

        public void Reset(int random)
        {
            hit = false;

            stopped = true;

            leftShrink = -1;
            rightShrink = 1;

            InitializeVertices(random);

            if (random == 0)
            {
                lane = -8;
            }
            else
            {
                lane = 8;
            }

        }
    }
}
