# Font and Image Component
## Fonts
In Next.js, you can customize fonts by adding external fonts, using local font files, or using a CSS-in-JS approach. Here’s a step-by-step guide for each method.

### Method 1: Using Google Fonts (External Fonts)

1. **Install `next/font` (built-in in Next.js 13 and later) for optimized font loading**:
   
   Next.js offers a built-in font optimization feature starting with Next.js 13. You can easily import and use Google Fonts without worrying about performance.

   Example:
   ```js
   import { Inter } from 'next/font/google';

   const inter = Inter({
     subsets: ['latin'],
     weight: ['400', '700'],
   });

   export default function Home() {
     return (
       <div style={{ fontFamily: inter.style.fontFamily }}>
         <h1>Hello, World!</h1>
       </div>
     );
   }
   ```

2. **Custom Fonts Using CSS**:
   If you're using external fonts like Google Fonts manually, you can include the link in the `<Head>` component, then apply it in your CSS.

   Example:
   ```jsx
   import Head from 'next/head';

   export default function Home() {
     return (
       <>
         <Head>
           <link
             href="https://fonts.googleapis.com/css2?family=Inter:wght@400;700&display=swap"
             rel="stylesheet"
           />
         </Head>
         <div style={{ fontFamily: 'Inter, sans-serif' }}>
           <h1>Hello, World!</h1>
         </div>
       </>
     );
   }
   ```

### Method 2: Using Local Font Files

1. **Store font files in the `public` folder**: Place your font files (e.g., `.woff`, `.woff2`, `.ttf`) in the `public` directory of your Next.js project.

2. **Include the fonts using CSS**:
   In your global CSS or a specific component’s style, you can reference the local fonts.

   Example:
   ```css
   /* styles/global.css */
   @font-face {
     font-family: 'MyCustomFont';
     src: url('/fonts/MyCustomFont.woff2') format('woff2'),
          url('/fonts/MyCustomFont.woff') format('woff');
     font-weight: normal;
     font-style: normal;
   }

   body {
     font-family: 'MyCustomFont', sans-serif;
   }
   ```

3. **Import your CSS file**:
   Make sure your global CSS file is imported in `_app.js` or `_app.tsx`.

   ```jsx
   import '../styles/global.css';
   ```

### Method 3: Using CSS-in-JS (Styled Components or Emotion)

If you prefer CSS-in-JS solutions, you can use libraries like **styled-components** or **@emotion/react** for adding custom fonts.

1. **Install Styled Components**:
   ```bash
   npm install styled-components
   npm install @types/styled-components (for TypeScript users)
   ```

2. **Create a styled component with custom fonts**:

   Example:
   ```jsx
   import styled from 'styled-components';

   const Heading = styled.h1`
     font-family: 'Inter', sans-serif;
     font-weight: 700;
   `;

   export default function Home() {
     return (
       <Heading>
         Hello, World!
       </Heading>
     );
   }
   ```

### Method 4: Using CSS Modules

If you're using CSS modules in your Next.js project, you can define custom fonts just like with global CSS but scoped to a specific component.

1. **Create a CSS module file**:
   Create a file like `Home.module.css`.

   Example:
   ```css
   /* Home.module.css */
   .heading {
     font-family: 'Inter', sans-serif;
     font-weight: 700;
   }
   ```

2. **Import and use the module**:
   In your component, import the CSS module and apply it.

   ```jsx
   import styles from './Home.module.css';

   export default function Home() {
     return (
       <h1 className={styles.heading}>Hello, World!</h1>
     );
   }
   ```

## Image Component
In **Next.js**, the recommended way to handle images is using the built-in `<Image />` component from `next/image`. This component optimizes images automatically for better performance (e.g., lazy loading, optimized sizing, and responsive support).

### **Step 1: Install Next.js (if you haven't already)**
```bash
npx create-next-app@latest my-next-app
cd my-next-app
```

---

### **Step 2: Use the `next/image` Component**
Edit your component (e.g., `pages/index.js`):

```javascript
// pages/index.js
import Image from 'next/image';

export default function Home() {
  return (
    <div>
      <h1>Welcome to My Next.js App</h1>
      
      {/* Local Image */}
      <Image 
        src="/images/my-photo.jpg" 
        alt="My Photo" 
        width={300} 
        height={300} 
        priority 
      />

      {/* Remote Image */}
      <Image 
        src="https://example.com/photo.jpg" 
        alt="Remote Photo" 
        width={400} 
        height={300} 
      />

    </div>
  );
}
```

---

### **Step 3: Configure `next.config.js` for External Images**
If you are using remote images, add their domains to your `next.config.js`:

```javascript
// next.config.js
module.exports = {
  images: {
    domains: ['example.com'], // Add the remote image domains here
  },
};
```

---

### **Key Props of `<Image />`:**
- `src`: The image URL or path (required).
- `alt`: Description for accessibility (required).
- `width` & `height`: Fixed size (recommended for layout stability).
- `priority`: Loads the image immediately (use for above-the-fold images).
- `layout`: Supports values like `fill`, `intrinsic`, or `responsive`.

---

### **Bonus: Use `layout="fill"` for Responsive Images**
```javascript
<div style={{ position: 'relative', width: '100%', height: '400px' }}>
  <Image 
    src="/images/background.jpg" 
    alt="Background" 
    layout="fill" 
    objectFit="cover"
  />
</div>
```

---

### **Final Notes**:
- **`next/image`** automatically optimizes images for all screen sizes.
- It also **lazy-loads** images by default for performance.
- Add `loader` functions for **custom CDN support** if needed.