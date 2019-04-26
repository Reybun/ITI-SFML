using SFML.System;

namespace SFML.Graphics
{
    /// <summary>
    /// Render targets (renderwindow, renderimage) abstraction.
    /// </summary>
    public interface IRenderTarget
    {
        /// <summary>
        /// Gets the size of the rendering region of the target.
        /// </summary>
        Vector2u Size { get; }

        /// <summary>
        /// Gets the default view of the render target.
        /// The default view has the initial size of the render target, and
        /// never changes after the target has been created.
        /// </summary>
        View DefaultView { get; }


        /// <summary>
        /// Gets or sets the current active view.
        /// </summary>
        View View { get; set; }

        /// <summary>
        /// Gets the viewport of a view applied to this target.
        /// </summary>
        /// <param name="view">Target view.</param>
        /// <returns>Viewport rectangle, expressed in pixels in the current target.</returns>
        IntRect GetViewport( View view );

        /// <summary>
        /// Converts a point from target coordinates to world
        /// coordinates, using the current view.
        /// <para>
        /// This function is an overload of the MapPixelToCoords
        /// function that implicitly uses the current view.
        /// It is equivalent to:
        /// target.MapPixelToCoords(point, target.GetView());
        /// </para>
        /// </summary>
        /// <param name="point">Pixel to convert.</param>
        /// <returns>The converted point, in "world" coordinates.</returns>
        Vector2f MapPixelToCoords( Vector2i point );

        /// <summary>
        /// Converts a point from target coordinates to world coordinates
        /// <para>
        /// This function finds the 2D position that matches the
        /// given pixel of the render-target. In other words, it does
        /// the inverse of what the graphics card does, to find the
        /// initial position of a rendered pixel.
        /// </para>
        /// <para>
        /// Initially, both coordinate systems (world units and target pixels)
        /// match perfectly. But if you define a custom view or resize your
        /// render-target, this assertion is not true anymore, ie. a point
        /// located at (10, 50) in your render-target may map to the point
        /// (150, 75) in your 2D world -- if the view is translated by (140, 25).
        /// </para>
        /// <para>
        /// For render-windows, this function is typically used to find
        /// which point (or object) is located below the mouse cursor.
        /// </para>
        /// <para>
        /// This version uses a custom view for calculations, see the other
        /// overload of the function if you want to use the current view of the
        /// render-target.
        /// </para>
        /// </summary>
        /// <param name="point">Pixel to convert.</param>
        /// <param name="view">The view to use for converting the point.</param>
        /// <returns>The converted point, in "world" coordinates.</returns>
        Vector2f MapPixelToCoords( Vector2i point, View view );

        /// <summary>
        /// Converts a point from world coordinates to target
        /// coordinates, using the current view.
        /// <para>
        /// This function is an overload of the mapCoordsToPixel
        /// function that implicitly uses the current view.
        /// It is equivalent to:
        /// target.MapCoordsToPixel(point, target.GetView());
        /// </para>
        /// </summary>
        /// <param name="point">Point to convert.</param>
        /// <returns>The converted point, in target coordinates (pixels).</returns>
        Vector2i MapCoordsToPixel( Vector2f point );

        /// <summary>
        /// Converts a point from world coordinates to target coordinates.
        /// <para>
        /// This function finds the pixel of the render-target that matches
        /// the given 2D point. In other words, it goes through the same process
        /// as the graphics card, to compute the final position of a rendered point.
        /// </para>
        /// <para>
        /// Initially, both coordinate systems (world units and target pixels)
        /// match perfectly. But if you define a custom view or resize your
        /// render-target, this assertion is not true anymore, ie. a point
        /// located at (150, 75) in your 2D world may map to the pixel
        /// (10, 50) of your render-target -- if the view is translated by (140, 25).
        /// </para>
        /// <para>
        /// This version uses a custom view for calculations, see the other
        /// overload of the function if you want to use the current view of the
        /// render-target.
        /// </para>
        /// </summary>
        /// <param name="point">Point to convert.</param>
        /// <param name="view">The view to use for converting the point.</param>
        /// <returns>The converted point, in target coordinates (pixels).</returns>
        Vector2i MapCoordsToPixel( Vector2f point, View view );

        /// <summary>
        /// Clears the entire target with black color.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears the entire target with a single color
        /// </summary>
        /// <param name="color">Color to use to clear the window</param>
        void Clear( Color color );

        /// <summary>
        /// Draws a drawable object to the render-target, with default render states
        /// </summary>
        /// <param name="drawable">Object to draw</param>
        void Draw( IDrawable drawable );

        /// <summary>
        /// Draws a drawable object to the render-target.
        /// </summary>
        /// <param name="drawable">Object to draw.</param>
        /// <param name="states">Render states to use for drawing.</param>
        void Draw( IDrawable drawable, RenderStates states );

        /// <summary>
        /// Draws primitives defined by an array of vertices, with default render states
        /// </summary>
        /// <param name="vertices">Array of vertices to draw.</param>
        /// <param name="type">Type of primitives to draw.</param>
        void Draw( Vertex[] vertices, PrimitiveType type );

        /// <summary>
        /// Draws primitives defined by an array of vertices.
        /// </summary>
        /// <param name="vertices">Array of vertices to draw.</param>
        /// <param name="type">Type of primitives to draw.</param>
        /// <param name="states">Render states to use for drawing.</param>
        void Draw( Vertex[] vertices, PrimitiveType type, RenderStates states );

        /// <summary>
        /// Draws primitives defined by a sub-array of vertices, with default render states.
        /// </summary>
        /// <param name="vertices">Array of vertices to draw.</param>
        /// <param name="start">Index of the first vertex to draw in the array.</param>
        /// <param name="count">Number of vertices to draw.</param>
        /// <param name="type">Type of primitives to draw.</param>
        void Draw( Vertex[] vertices, uint start, uint count, PrimitiveType type );

        /// <summary>
        /// Draws primitives defined by a sub-array of vertices.
        /// </summary>
        /// <param name="vertices">Pointer to the vertices.</param>
        /// <param name="start">Index of the first vertex to use in the array.</param>
        /// <param name="count">Number of vertices to draw.</param>
        /// <param name="type">Type of primitives to draw.</param>
        /// <param name="states">Render states to use for drawing.</param>
        void Draw( Vertex[] vertices, uint start, uint count, PrimitiveType type, RenderStates states );

        /// <summary>
        /// Save the current OpenGL render states and matrices.
        /// <para>
        /// This function can be used when you mix SFML drawing
        /// and direct OpenGL rendering. Combined with PopGLStates,
        /// it ensures that:
        /// - SFML's internal states are not messed up by your OpenGL code
        /// - your OpenGL states are not modified by a call to a SFML function
        /// </para>
        /// <para>
        /// More specifically, it must be used around code that
        /// calls Draw functions. Example:
        /// <code>
        /// // OpenGL code here...
        /// window.PushGLStates();
        /// window.Draw(...);
        /// window.Draw(...);
        /// window.PopGLStates();
        /// // OpenGL code here...
        /// </code>
        /// </para>
        /// <para>
        /// Note that this function is quite expensive: it saves all the
        /// possible OpenGL states and matrices, even the ones you
        /// don't care about. Therefore it should be used wisely.
        /// It is provided for convenience, but the best results will
        /// be achieved if you handle OpenGL states yourself (because
        /// you know which states have really changed, and need to be
        /// saved and restored). Take a look at the ResetGLStates
        /// function if you do so.
        /// </para>
        /// </summary>
        void PushGLStates();

        /// <summary>
        /// Restore the previously saved OpenGL render states and matrices.
        /// See the description of PushGLStates to get a detailed
        /// description of these functions.
        /// </summary>
        void PopGLStates();

        /// <summary>
        /// Reset the internal OpenGL states so that the target is ready for drawing.
        /// <para>
        /// This function can be used when you mix SFML drawing
        /// and direct OpenGL rendering, if you choose not to use
        /// PushGLStates/PopGLStates. It makes sure that all OpenGL
        /// states needed by SFML are set, so that subsequent Draw()
        /// calls will work as expected. Example:
        /// <code>
        /// // OpenGL code here...
        /// glPushAttrib(...);
        /// window.ResetGLStates();
        /// window.Draw(...);
        /// window.Draw(...);
        /// glPopAttrib(...);
        /// // OpenGL code here...
        /// </code>
        /// </para>
        /// </summary>
        void ResetGLStates();
    }
}
