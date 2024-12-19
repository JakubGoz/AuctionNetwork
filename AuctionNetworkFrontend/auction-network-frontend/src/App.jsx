import { BrowserRouter, Routes, Route } from "react-router-dom";
import './App.module.scss';
import 'bootstrap/dist/css/bootstrap.min.css';
import AuthenticationPage from "./Authentication/AuthenticationPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/*"
          element={
            <>
              <AuthenticationPage />
            </>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App

