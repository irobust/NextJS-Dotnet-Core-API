# Pages and Link Navigation
In Next.js, creating pages and setting up navigation is quite straightforward. Here’s how you can do it step by step:

### 1. Creating Pages

In Next.js, pages are created automatically based on directory. Each folder name that has `page.jsx` (or `page.tsx` for TypeScript) file becomes a route.

#### Example: 
- Create a file `home/page.tsx` for the homepage.
- Create another file `about/page.tsx` for the "About" page.

```js
// home/page.tsx
export default function Home() {
  return (
    <div>
      <h1>Welcome to the Home Page!</h1>
    </div>
  );
}
```

```js
// about/page.tsx
export default function About() {
  return (
    <div>
      <h1>About Us</h1>
    </div>
  );
}
```

Now, `/` will display the homepage and `/about` will display the About page.

### 2. Linking Between Pages

Next.js provides a built-in component called `Link` for client-side navigation. It helps to link between pages without reloading the page, making navigation more seamless.

#### Example:

```js
// pages/index.js
import Link from 'next/link';

export default function Home() {
  return (
    <div>
      <h1>Welcome to the Home Page!</h1>
      <Link href="/about">
        <a>Go to About</a>
      </Link>
    </div>
  );
}
```

This will render a clickable link that navigates to the `/about` page when clicked.

### 3. Dynamic Routes

If you want to create dynamic routes (e.g., for a blog post), you can do this by adding square brackets to the file name inside the `posts` folder.

#### Example:
- `posts/[id].js` would capture dynamic routes like `/posts/1`, `/posts/2`, etc.

```js
// posts/[id].js

export default async function Post({params} : {
  params : Promise<{ id: string }>
}) {
  const id = (await params).id
  return (
    <div>
      <h1>Post ID: {id}</h1>
    </div>
  );
}
```

Now, navigating to `/posts/1` will show the post with ID `1`, and so on.

### 4. Custom Navigation Bar (Optional)

To make navigation easier, you can create a custom navigation bar that links to various pages.

```js
// components/NavBar.js
import Link from 'next/link';

export default function NavBar() {
  return (
    <nav>
      <Link href="/"><a>Home</a></Link>
      <Link href="/about"><a>About</a></Link>
    </nav>
  );
}
```

And use it inside your pages like so:

```js
// pages/index.js
import NavBar from '../components/NavBar';

export default function Home() {
  return (
    <div>
      <NavBar />
      <h1>Welcome to the Home Page!</h1>
    </div>
  );
}
```

### Key Points:
- **Pages** are created by adding `page.jsx` or `page.tsx` files to the directory.
- **Linking** between pages is done using the `Link` component.
- **Dynamic routes** can be created with file names like `[id].js`.
- You can create a **navigation bar** with links to navigate between pages.

That's all for basic page creation and navigation! Let me know if you need further clarification or examples.