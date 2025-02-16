# Create Next.js Project
To create a **Next.js** project, follow these steps:

### 1. **Ensure Node.js is Installed**
First, check if you have Node.js installed:
```bash
node -v
```
If not, [download and install Node.js](https://nodejs.org/).

---

### 2. **Create a Next.js App**
You can use the `create-next-app` command:
```bash
npx create-next-app@latest my-next-app
```
- `npx`: Runs the package without installing it globally.
- `create-next-app`: The official Next.js scaffolding tool.
- `my-next-app`: The name of your project folder (change it if you want).

---

### 3. **Answer the question**
```
    ✔ Would you like to use TypeScript? … Yes
    ✔ Would you like to use ESLint? … Yes
    ✔ Would you like to use Tailwind CSS? … Yes
    ✔ Would you like your code inside a `src/` directory? … No
    ✔ Would you like to use App Router? (recommended) … Yes
    ✔ Would you like to use Turbopack for `next dev`? … Yes
    ✔ Would you like to customize the import alias (`@/*` by default)? … No
```

### 3. **Navigate into the Project Directory**
```bash
cd my-next-app
```

---

### 4. **Start the Development Server**
```bash
npm run dev
```

- By default, the development server runs on [http://localhost:3000](http://localhost:3000).

---

### 5. **Visit Your Next.js App**
Open your browser and go to:
```
http://localhost:3000
```

---

### 6. **Build for Production (Optional)**
When you're ready to deploy:
```bash
npm run build
npm run start
```

---

### 7. **Using TypeScript (Optional)**
To create a TypeScript project:
```bash
npx create-next-app@latest my-next-app --typescript
```

---

### **Common Commands:**
- `npm run dev` — Start the development server.
- `npm run build` — Create a production build.
- `npm run start` — Start the production server.
- `npm run lint` — Run ESLint for code quality.