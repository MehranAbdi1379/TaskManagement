import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App.tsx";
import { Provider } from "./components/ui/provider.tsx";

createRoot(document.getElementById("root")!).render(
  <Provider>
    <StrictMode>
      <App />
    </StrictMode>
  </Provider>
);
