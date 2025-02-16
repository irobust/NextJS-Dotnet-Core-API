# CSS Styles in Next.js
To use both regular CSS and Tailwind CSS in a Next.js project, follow these steps:

### 1. **Install Tailwind CSS**

First, you need to install Tailwind CSS in your Next.js project. If you haven't already done this, you can set it up by following these steps:

- **Step 1: Install Tailwind CSS dependencies:**

```bash
npm install tailwindcss postcss autoprefixer
```

- **Step 2: Initialize Tailwind CSS configuration files:**

```bash
npx tailwindcss init -p
```

This will create two files:
- `tailwind.config.js` for customizing Tailwind settings.
- `postcss.config.js` for PostCSS configuration.

- **Step 3: Configure Tailwind CSS:**

In the `tailwind.config.js`, set the `content` property to point to your JSX/TSX files, typically:

```javascript
module.exports = {
  content: [
    './pages/**/*.{js,ts,jsx,tsx}',
    './components/**/*.{js,ts,jsx,tsx}',
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

- **Step 4: Add Tailwind to your CSS file:**

In your global CSS file (e.g., `styles/globals.css`), add the following lines to enable Tailwind:

```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

### 2. **Using Regular CSS in Next.js**

To use custom CSS alongside Tailwind, you can simply create CSS files and import them into your Next.js components or pages.

- **Step 1: Create a CSS file:**

For example, create a file called `styles/custom.css` and add some custom styles:

```css
/* styles/custom.css */
.custom-class {
  background-color: lightblue;
  padding: 10px;
  border-radius: 5px;
}
```

- **Step 2: Import the CSS in your component or page:**

```jsx
import '../styles/custom.css';

export default function Home() {
  return (
    <div>
      <div className="custom-class">
        This is a custom styled div
      </div>
    </div>
  );
}
```

### 3. **Using Tailwind CSS in Next.js**

To use Tailwind classes, simply apply them to the elements in your JSX components.

```jsx
export default function Home() {
  return (
    <div className="bg-blue-500 text-white p-4 rounded-lg">
      This is a Tailwind-styled div
    </div>
  );
}
```

### 4. **Combining Regular CSS and Tailwind CSS**

You can combine custom CSS with Tailwind classes. Tailwind CSS provides utility-first classes, while your regular CSS file can handle more complex or custom styles.

For example:

```jsx
import '../styles/custom.css';

export default function Home() {
  return (
    <div>
      <div className="bg-green-500 text-white p-4 rounded-md">
        This is styled with Tailwind
      </div>
      <div className="custom-class">
        This is styled with regular CSS
      </div>
    </div>
  );
}
```

### 5. **Important Notes**

- When you use Tailwind CSS, you usually rely on utility classes for styling, but you can still write custom styles for more complex or reusable components using regular CSS.
- Next.js automatically supports CSS imports from your components or pages, so it's easy to mix Tailwind CSS and regular CSS in the same project.

With these steps, you can set up and use both Tailwind CSS and regular CSS in a Next.js project!