import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import './App.module.scss';
import Layout from "./Layout";
import HomePage from "./Home/HomePage";
import CreateListingPage from "./CreateListing/CreateListingPage";
import AuthenticationPage from "./Authentication/AuthenticationPage";
import ListingsPage from "./Listings/ListingsPage"; // Import nowego komponentu
import ListingDetails from "./Listings/ListingDetails/ListingDetails";
import OrderPaymentPage from './OrderPayment/OrderPaymentPage'; // Import nowej strony
import './App.module.scss';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useState, useEffect } from "react";
import axios from "axios";
import { baseUrl, authorization } from "./Shared/Options/ApiOptions";

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(null);

  useEffect(() => {
    const checkIfUserIsLoggedIn = async () => {
      if (localStorage.getItem("token")) {
        try {
          await axios.get(
            `${baseUrl}/user/is-logged-in`,
            authorization(localStorage.getItem("token"))
          );
          setIsAuthenticated(true);
        } catch (err) {
          setIsAuthenticated(false);
        }
      } else {
        setIsAuthenticated(false);
      }
    };
    checkIfUserIsLoggedIn();
  }, []);

  if (isAuthenticated === null) {
    return <div>Loading...</div>;
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/"
          element={
            isAuthenticated ? (
              <Navigate to="/home" replace={true} />
            ) : (
              <Navigate to="/auth" replace={true} />
            )
          }
        />
        <Route element={<Layout isAuthenticated={isAuthenticated} />}>
          <Route
            path="/home"
            element={isAuthenticated ? <HomePage /> : <Navigate to="/home" replace />}
          />
        
          <Route
            path="/CreateListing"
            element={isAuthenticated ? <CreateListingPage /> : <Navigate to="/auth" replace />}
          />
          <Route 
            path="/create-listing/:listingId" 
            element={isAuthenticated ? <CreateListingPage /> : <Navigate to="/auth" replace />} 
          />
          
          <Route
            path="/order-payment/:listingId"
            element={isAuthenticated ? <OrderPaymentPage /> : <Navigate to="/auth" replace />}
          />
          
          <Route
            path="/categories/:categoryId"
            element={isAuthenticated ? <ListingsPage /> : <Navigate to="/auth" replace />}
          />
          <Route
            path="/listing/:id"
            element={isAuthenticated ? <ListingDetails /> : <Navigate to="/auth" replace />}
          />
          <Route 
            path="/listing/user/:userId" 
            element={isAuthenticated ? <ListingsPage /> : <Navigate to="/auth" replace />} 
          />
          <Route 
            path="/listing/purchases" 
            element={isAuthenticated ? <ListingsPage /> : <Navigate to="/auth" replace />} 
          />
          <Route 
            path="/listing/sales" 
            element={isAuthenticated ? <ListingsPage /> : <Navigate to="/auth" replace />} 
          />
          <Route 
            path="/bid/myitems" 
            element={isAuthenticated ? <ListingsPage /> : <Navigate to="/auth" replace />} 
          />
          <Route
            path="/listing/results"
            element={isAuthenticated ? <ListingsPage /> : <Navigate to="/auth" replace />}
          />
        </Route>

        <Route path="/auth" element={!isAuthenticated ? <AuthenticationPage /> : <Navigate to="/" replace />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
