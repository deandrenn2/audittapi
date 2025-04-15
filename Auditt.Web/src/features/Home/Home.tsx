import { DashboradStatistcs } from "../Dashboard/DashbordStatistcs";



export const Home = () => {


   return (
      <div>
         {/* <!-- Cards Section --> */}
         <div className="p-4 border border-grey-500 ">
            <div>
               <DashboradStatistcs />
            </div>
         </div>
      </div>

   );
};
