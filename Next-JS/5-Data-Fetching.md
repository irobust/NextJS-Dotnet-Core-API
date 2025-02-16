# Data Fetching in Next.js
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