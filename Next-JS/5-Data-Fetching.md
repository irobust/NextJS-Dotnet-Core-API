# Data Fetching in Next.js
## Fetch Data
In Next.js, you can use the `fetch()` API for making HTTP requests, whether on the client or server side. Here’s how to use `fetch()` properly in different contexts within a Next.js application:

---

## **1. Fetching Data in `getServerSideProps` (SSR)**  
Fetches data on every request (useful for frequently updating data).

### Example:
```javascript
// pages/posts.js
export async function getServerSideProps() {
  const res = await fetch('https://jsonplaceholder.typicode.com/posts');
  const posts = await res.json();

  return {
    props: { posts },
  };
}

export default function Posts({ posts }) {
  return (
    <div>
      <h1>Posts</h1>
      <ul>
        {posts.map(post => (
          <li key={post.id}>{post.title}</li>
        ))}
      </ul>
    </div>
  );
}
```

---

## **2. Fetching Data in `getStaticProps` (SSG)**  
Pre-renders the page at build time (useful for static content).

### Example:
```javascript
// pages/posts.js
export async function getStaticProps() {
  const res = await fetch('https://jsonplaceholder.typicode.com/posts');
  const posts = await res.json();

  return {
    props: { posts },
    revalidate: 10, // Rebuild every 10 seconds (ISR)
  };
}

export default function Posts({ posts }) {
  return (
    <div>
      <h1>Posts</h1>
      <ul>
        {posts.map(post => (
          <li key={post.id}>{post.title}</li>
        ))}
      </ul>
    </div>
  );
}
```

---

## **3. Fetching Data in API Routes**
Create custom backend endpoints using Next.js API routes.

### Example:
```javascript
// pages/api/posts.js
export default async function handler(req, res) {
  const response = await fetch('https://jsonplaceholder.typicode.com/posts');
  const posts = await response.json();
  res.status(200).json(posts);
}
```

---

## **4. Fetching Data on the Client Side**
For user interactions and dynamic content that don’t need pre-rendering.

### Example:
```javascript
// pages/posts.js
import { useEffect, useState } from 'react';

export default function Posts() {
  const [posts, setPosts] = useState([]);

  useEffect(() => {
    fetch('/api/posts') // Use your own API route or external URL
      .then(response => response.json())
      .then(data => setPosts(data))
      .catch(error => console.error('Error:', error));
  }, []);

  return (
    <div>
      <h1>Client-Side Posts</h1>
      <ul>
        {posts.map(post => (
          <li key={post.id}>{post.title}</li>
        ))}
      </ul>
    </div>
  );
}
```

---

## SWR
SWR is a React hook library for data fetching that makes it easy to fetch and cache data in React applications. It stands for "stale-while-revalidate", and it handles caching, re-fetching, and error handling for you.

Here’s how you can fetch data using SWR:

### 1. Install SWR

You need to install the SWR library first:

```bash
npm install swr
```

### 2. Use SWR to Fetch Data

Here’s how you can use SWR to fetch data:

```javascript
import useSWR from 'swr';

// Define a fetcher function that SWR will use to fetch data
const fetcher = (url) => fetch(url).then((res) => res.json());

function MyComponent() {
  const { data, error } = useSWR('https://api.example.com/data', fetcher);

  if (error) return <div>Failed to load</div>;
  if (!data) return <div>Loading...</div>;

  return (
    <div>
      <h1>Data:</h1>
      <pre>{JSON.stringify(data, null, 2)}</pre>
    </div>
  );
}
```

### Explanation:
1. `useSWR` is used to fetch data. The first argument is the URL or the key (the source of the data).
2. The second argument is the `fetcher` function that fetches the data and returns a promise (usually using `fetch`).
3. `useSWR` returns an object containing:
   - `data`: The fetched data.
   - `error`: If there’s an error during fetching.

### 3. Handling Loading, Error, and Success States
You can easily handle loading and error states:
- `if (!data)`: Handles the loading state.
- `if (error)`: Handles errors.

### 4. Optional Configuration
SWR provides a second argument where you can pass configurations for things like revalidation and caching. Here's an example:

```javascript
const { data, error } = useSWR('https://api.example.com/data', fetcher, {
  revalidateOnFocus: false, // disable revalidation when window gets focus
  refreshInterval: 5000,    // refresh data every 5 seconds
});
```

### Key SWR Features:
- **Caching**: Automatically caches responses, so you don’t have to refetch data if it hasn’t changed.
- **Re-fetching**: SWR automatically re-fetches data when needed (for example, on window focus).
- **Stale While Revalidate**: SWR shows stale data while fetching fresh data in the background.
- **Pagination/Infinite Scrolling**: SWR supports paginated and infinite scrolling data fetching out of the box.