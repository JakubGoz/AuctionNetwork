import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import axios from 'axios';
import ReactPaginate from 'react-paginate';
import { baseUrl } from '../Shared/Options/ApiOptions';
import paginationClasses from '../Shared/pagination.module.scss';
import ListingComponent from './Components/ListingComponent';
import debounce from 'lodash.debounce';

const ListingsPage = () => {
  const [listings, setListings] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const { categoryId, userId } = useParams(); 
  const location = useLocation(); // Bieżąca lokalizacja
  const filters = location.state?.filters || {}; // Pobieranie filtrów z Layout

  const navigate = useNavigate();

  const fetchListings = async (currentPage, filters) => {
    try {
      const token = localStorage.getItem('token');
      const params = { PageNumber: currentPage, ...filters };
      let response;

      if (categoryId) {
        response = await axios.get(`${baseUrl}/listing/category/${categoryId}`, {
          params,
          headers: { Authorization: `Bearer ${token}` },
        });
      } else if (userId) {
        response = await axios.get(`${baseUrl}/listing/user/${userId}`, {
          params,
          headers: { Authorization: `Bearer ${token}` },
        });
      } else if (location.pathname === '/listing/purchases') {
        response = await axios.get(`${baseUrl}/listing/purchases`, {
          params,
          headers: { Authorization: `Bearer ${token}` },
        });
      } else if (location.pathname === '/listing/sales') {
        response = await axios.get(`${baseUrl}/listing/sales`, {
          params,
          headers: { Authorization: `Bearer ${token}` },
        });
      } else if (location.pathname === '/bid/myitems') {
        response = await axios.get(`${baseUrl}/bid/myitems`, {
          params,
          headers: { Authorization: `Bearer ${token}` },
        });
      } else if (location.pathname === '/listing/results') {
        response = await axios.get(`${baseUrl}/listing/results`, {
          params: filters,
          headers: { Authorization: `Bearer ${token}` },
        });
      }
      setListings(response.data.items);
      setTotalPages(response.data.totalPages);

    } catch (error) {
      console.error('Error fetching listings:', error.message);
    }
  };

  const debouncedFetchListings = debounce(fetchListings, 300); // Debouncing z opóźnieniem 500ms

  useEffect(() => {
    debouncedFetchListings(pageNumber, filters); // Użycie debounced fetch
    return () => debouncedFetchListings.cancel(); // Anulowanie opóźnionego wywołania, jeśli komponent jest odmontowywany
  }, [pageNumber, categoryId, userId, filters]);

  const handlePageChange = ({ selected }) => {
    const newPageNumber = selected + 1; // Pamiętaj, że paginacja jest 1-indexed
    setPageNumber(newPageNumber);
  };

  return (
    <div>
      <div className="d-flex justify-content-center">
        <ul>
          {listings.map((listing) => (
            <ListingComponent key={listing.listingId} listing={listing} />
          ))}
        </ul>
      </div>
      <ReactPaginate
        pageCount={totalPages}
        pageRangeDisplayed={3}
        marginPagesDisplayed={1}
        onPageChange={handlePageChange}
        containerClassName={paginationClasses.pagination}
        activeClassName={paginationClasses.active}
        disabledClassName={paginationClasses.disabled}
      />
    </div>
  );
};

export default ListingsPage;
