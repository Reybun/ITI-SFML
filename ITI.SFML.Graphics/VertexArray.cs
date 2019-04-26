using SFML.System;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SFML.Graphics
{
    /// <summary>
    /// Defines a set of one or more 2D primitives.
    /// </summary>
    public class VertexArray : ObjectBase, IDrawable
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public VertexArray()
            : base( sfVertexArray_create() )
        {
        }

        /// <summary>
        /// Constructs the vertex array with a type.
        /// </summary>
        /// <param name="type">Type of primitives.</param>
        public VertexArray( PrimitiveType type )
            : base( sfVertexArray_create() )
        {
            PrimitiveType = type;
        }

        /// <summary>
        /// Constructs the vertex array with a type and an initial number of vertices.
        /// </summary>
        /// <param name="type">Type of primitives.</param>
        /// <param name="vertexCount">Initial number of vertices in the array.</param>
        public VertexArray( PrimitiveType type, uint vertexCount )
            : base( sfVertexArray_create() )
        {
            PrimitiveType = type;
            Resize( vertexCount );
        }

        /// <summary>
        /// Constructs the vertex array from another vertex array.
        /// </summary>
        /// <param name="copy">Transformable to copy.</param>
        public VertexArray( VertexArray copy )
            : base( sfVertexArray_copy( copy.CPointer ) )
        {
        }

        /// <summary>
        /// Gets the total vertex count.
        /// </summary>
        public uint VertexCount => sfVertexArray_getVertexCount( CPointer );

        /// <summary>
        /// Read-write access to vertices by their index.
        /// <para>
        /// This function doesn't check index, it must be in range
        /// [0, VertexCount - 1]. The behaviour is undefined
        /// otherwise.
        /// </para>
        /// </summary>
        /// <param name="index">Index of the vertex to get.</param>
        /// <returns>Reference to the index-th vertex.</returns>
        ////////////////////////////////////////////////////////////
        public Vertex this[uint index]
        {
            get
            {
                unsafe
                {
                    return *sfVertexArray_getVertex( CPointer, index );
                }
            }
            set
            {
                unsafe
                {
                    *sfVertexArray_getVertex( CPointer, index ) = value;
                }
            }
        }

        /// <summary>
        /// Clears the vertex array
        /// </summary>
        public void Clear() => sfVertexArray_clear( CPointer );

        /// <summary>
        /// Resizes the vertex array.
        /// <para>
        /// If vertexCount is greater than the current size, the previous
        /// vertices are kept and new (default-constructed) vertices are
        /// added.
        /// </para>
        /// <para>
        /// If vertexCount is less than the current size, existing vertices
        /// are removed from the array.
        /// </para>
        /// </summary>
        /// <param name="vertexCount">New size of the array (number of vertices).</param>
        public void Resize( uint vertexCount ) => sfVertexArray_resize( CPointer, vertexCount );

        /// <summary>
        /// Appends a vertex to the array.
        /// </summary>
        /// <param name="vertex">Vertex to add</param>
        public void Append( Vertex vertex ) => sfVertexArray_append( CPointer, vertex );

        /// <summary>
        /// Gets or sets th type of primitives to draw.
        /// </summary>
        public PrimitiveType PrimitiveType
        {
            get { return sfVertexArray_getPrimitiveType( CPointer ); }
            set { sfVertexArray_setPrimitiveType( CPointer, value ); }
        }

        /// <summary>
        /// Computes the bounding rectangle of the vertex array.
        /// <para>
        /// This function returns the axis-aligned rectangle that
        /// contains all the vertices of the array.
        /// </para>
        /// </summary>
        public FloatRect Bounds => sfVertexArray_getBounds( CPointer );

        /// <summary>
        /// Draws the vertex array to a render target.
        /// </summary>
        /// <param name="target">Render target to draw to.</param>
        /// <param name="states">Current render states.</param>
        public void Draw( IRenderTarget target, in RenderStates states )
        {
            RenderStates.MarshalData marshaled = states.Marshal();
            switch (target)
            {
                case RenderWindow window:
                    sfRenderWindow_drawVertexArray( window.CPointer, CPointer, ref marshaled );
                    break;
                case RenderTexture texture:
                    sfRenderTexture_drawVertexArray( texture.CPointer, CPointer, ref marshaled );
                    break;
            }
        }

        /// <summary>
        /// Handles the destruction of the object.
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void OnDispose( bool disposing )
        {
            sfVertexArray_destroy( CPointer );
        }

        #region Imports
        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfVertexArray_create();

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfVertexArray_copy( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfVertexArray_destroy( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfVertexArray_getVertexCount( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern unsafe Vertex* sfVertexArray_getVertex( IntPtr CPointer, uint index );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfVertexArray_clear( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfVertexArray_resize( IntPtr CPointer, uint vertexCount );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfVertexArray_append( IntPtr CPointer, Vertex vertex );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfVertexArray_setPrimitiveType( IntPtr CPointer, PrimitiveType type );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern PrimitiveType sfVertexArray_getPrimitiveType( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern FloatRect sfVertexArray_getBounds( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderWindow_drawVertexArray( IntPtr CPointer, IntPtr VertexArray, ref RenderStates.MarshalData states );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderTexture_drawVertexArray( IntPtr CPointer, IntPtr VertexArray, ref RenderStates.MarshalData states );
        #endregion
    }
}
