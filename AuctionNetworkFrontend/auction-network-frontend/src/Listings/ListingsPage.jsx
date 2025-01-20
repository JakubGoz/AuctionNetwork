// ListingsPage.jsx
import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom'; // Dodajemy useParams
import axios from 'axios';
import ReactPaginate from 'react-paginate';
import { baseUrl } from '../Shared/Options/ApiOptions';
import paginationClasses from '../Shared/pagination.module.scss';
import ListingComponent from './Components/ListingComponent';

const ListingsPage = () => {
  const [listings, setListings] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const { categoryId } = useParams(); // Pobieramy categoryId z URL

  const fetchListings = async (currentPage) => {
    try {
      const token = localStorage.getItem('token');
      const params = { PageNumber: currentPage };

      const response = await axios.get(`${baseUrl}/listing/category/${categoryId}`, {
        params,
        headers: { Authorization: `Bearer ${token}` },
      });

      setListings(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      console.error(error.message);
    }
  };

  useEffect(() => {
    fetchListings(pageNumber);
  }, [pageNumber, categoryId]);

  const handlePageChange = ({ selected }) => {
    const newPageNumber = selected + 1;
    setPageNumber(newPageNumber);
  };

  return (
    <div>
      <div className='d-flex justify-content-center'>
        <ul>
          {listings.map(listing => (
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
