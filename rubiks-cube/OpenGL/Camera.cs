﻿using OpenTK;
using System;

namespace OpenGL
{
    struct VMouse
    {
        public int x;
        public int y;
        public bool isDown;
    }

    public class Camera
    {
        //lighting Related
        public Matrix4 LightSpace { get; set; }
        public int ShadowMap { get; set; }
        public Vector3 LightPos { get; set; }

        public Matrix4 Transfrom { get; set; }
        public Matrix4 Projection { get; set; }
        public float AngleX { get; private set; }
        public float AngleY { get; private set; }
        public float Depth { get; private set; }

        public float MinDepth { get; set; }
        public float MaxDepth { get; set; }

        private Vector3 position;

        private Vector3 lookAt;

        private Vector3 offset;

        private VMouse mouse;

        public Camera(float _depth)
        {
            this.LookAt = new Vector3(0, 0, 0);
            this.Position = new Vector3(0, 0, 0);
            this.offset = new Vector3(0, 0, 0);
            this.Depth = _depth;

            MinDepth = 0;
            MaxDepth = 100;
        }

        public void update()
        {
            position = new Vector3((float)(Math.Sin(AngleX) * Math.Cos(AngleY) * Depth + offset.X), (float)(Math.Sin(AngleY) * Depth + offset.Y), (float)(Math.Cos(AngleX) * (float)Math.Cos(AngleY) * Depth + offset.Z));
            Transfrom = Matrix4.LookAt(position, LookAt, new Vector3(0, 1, 0));
        }

        public void setDepth(float min, float max)
        {
            this.MinDepth = min;
            this.MaxDepth = max;
        }

        public void mouseMove(int x, int y)
        {
            float dx = mouse.x - x;
            float dy = mouse.y - y;

            mouse.x = x;
            mouse.y = y;

            if (mouse.isDown)
            {
                AngleX += dx * (float)Math.PI / 180f / 4f;
                AngleY += -dy * (float)Math.PI / 180f / 4f;

                if (AngleY > 89f * Math.PI / 180f)
                    AngleY = 89f * (float)Math.PI / 180f;

                if (AngleY < -89f * Math.PI / 180f)
                    AngleY = -89f * (float)Math.PI / 180f;
            }
        }

        public void mouseUp()
        {
            mouse.isDown = false;
        }

        public void mouseDown(int x, int y)
        {
            mouse.isDown = true;
            mouse.x = x;
            mouse.y = y;
        }

        public void mouseWheel(int delta)
        {
            this.Depth += Depth * -delta * .02f;
            if (Depth > MaxDepth) Depth = MaxDepth;
            if (Depth < MinDepth) Depth = MinDepth;
        }

        public void keydown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.W)
            {
                this.offset.Y += .3f;
                this.lookAt.Y += .3f;
            }

            if (e.Key == OpenTK.Input.Key.S)
            {
                this.offset.Y -= .3f;
                this.lookAt.Y -= .3f;
            }

            if (e.Key == OpenTK.Input.Key.A)
            {
                Vector3 right = cross(new Vector3(0, 1, 0), this.position - this.lookAt).Normalized();
                this.offset -= right * .3f;
                this.lookAt -= right * .3f;
            }

            if (e.Key == OpenTK.Input.Key.D)
            {
                Vector3 right = cross(new Vector3(0, 1, 0), this.position - this.lookAt).Normalized();
                this.offset += right * .3f;
                this.lookAt += right * .3f;
            }

        }

        #region Properties
        public Vector3 Offset
        {
            get { return offset; }
            private set { this.offset = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public Vector3 LookAt
        {
            get { return lookAt; }
            private set { this.lookAt = value; }
        }

        public Vector3 cross(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y,
                                a.Z * b.X - a.X * b.Z,
                                a.X * b.Y - a.Y * b.X
                );
        }

        #endregion
    }
}
