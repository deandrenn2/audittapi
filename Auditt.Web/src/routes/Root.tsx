import { Sidebar } from '../layout/Sidebar';
import { Outlet, useNavigate } from 'react-router-dom';
import useUserContext from '../shared/context/useUserContext';
import { useAuth } from '../shared/context/useAuth';
import { useEffect } from 'react';

export default function Root() {
   const { setIsAuthenticated, setUser } = useUserContext();
   const { checkAuth, loading, user } = useAuth();
   const navigate = useNavigate();


   useEffect(() => {
      checkAuth();
   }, [checkAuth]);

   useEffect(() => {
      if (!user && !loading) {
         navigate('/login', { replace: true });
      } else {
         setIsAuthenticated(true);
         setUser(user);
      }
   }, [user, loading, navigate, setIsAuthenticated, setUser]);

   if (loading) {
      return <div className="flex justify-center items-center h-screen">Loading...</div>;
   }


   return (
      <div className=" bg-gray-100">
         <div
            id="main"
            className="flex bg-gray-900"
         >
            <Sidebar/>
            <div
               id="detail"
               className=" w-full bg-gray-100  min-h-screen">
               <Outlet/>
            </div>
         </div>

      </div>
   );
}
