import { Sidebar } from '../layout/Sidebar';
import { Outlet } from 'react-router-dom';
export default function Root() {
   // const { isAuthenticated } = useUserContext();
   // const navigate = useNavigate();

   // useEffect(() => {
   //    if (!isAuthenticated) {
   //       navigate('/login');
   //    }
   // }, [isAuthenticated, navigate]);

   return (
      <div  className=" bg-gray-100">   
         <div
            id="main"
            className="flex bg-gray-900 "
         >
            <Sidebar/>
            <div
               id="detail"
               className="  w-full bg-gray-100 flex min-h-screen">

               <Outlet />
            </div>
         </div>

      </div>
   );
}
