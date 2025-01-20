import React, { useState, useEffect } from 'react';
import axios from 'axios';
import ReactPaginate from 'react-paginate';
import { baseUrl } from '../Shared/Options/ApiOptions';
import classes from './UsersPage.module.scss';
import UserComponent from './Components/UserComponent/UserComponent';

const UsersPage = () => {
  const [users, setUsers] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [filters, setFilters] = useState({
    userName: '',
    country: '',
    city: ''
  });

  const fetchUsers = async (currentPage) => {
    try {
      const token = localStorage.getItem('token');

      const params = {
        UserName: filters.userName,
        Country: filters.country,
        City: filters.city,
        PageNumber: currentPage,
      };

      const response = await axios.get(`${baseUrl}/user`, {
        params,
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      setUsers(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      console.error(error.message);
    }
  };

  useEffect(() => {
    fetchUsers(pageNumber);
  }, [pageNumber]);

  const handleSearch = (e) => {
    e.preventDefault();
    setPageNumber(1);
    fetchUsers(1);
  };

  const handlePageChange = ({ selected }) => {
    const newPageNumber = selected + 1;
    setPageNumber(newPageNumber);
  };

  const handleInputChange = (e) => {
    setFilters({
      ...filters,
      [e.target.name]: e.target.value,
    });
  };

  return (
    <div>
      <form onSubmit={handleSearch}>
        <div className={classes['filter-input-group']}>
          <input
            type="text"
            name="userName"
            placeholder="User Name"
            className={classes["filter-input"]}
            value={filters.userName}
            onChange={handleInputChange}
          />
          <input
            type="text"
            name="country"
            placeholder="Country"
            className={classes["filter-input"]}
            value={filters.country}
            onChange={handleInputChange}
          />
          <input
            type="text"
            name="city"
            placeholder="City"
            className={classes["filter-input"]}
            value={filters.city}
            onChange={handleInputChange}
          />
          <button className={classes["search-button"]} type="submit">Search</button>
        </div>
      </form>
      <div className='d-flex justify-content-center'>
        {users.map(user => (
          <UserComponent key={user.userId} user={user} />
        ))}
      </div>
      <ReactPaginate
        pageCount={totalPages}
        pageRangeDisplayed={2}
        marginPagesDisplayed={2}
        onPageChange={handlePageChange}
        containerClassName={classes.pagination}
        activeClassName={classes.active}
        previousLabel={'<'}
        nextLabel={'>'}
        previousClassName={classes['pagination-arrow']}
        nextClassName={classes['pagination-arrow']}
        pageClassName={classes['pagination-button']}
      />
    </div>
  );
};

export default UsersPage;